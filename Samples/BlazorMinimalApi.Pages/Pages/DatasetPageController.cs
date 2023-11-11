using BlazorMinimalApis.Lib.Routing;
using BlazorMinimalApis.Pages.Auth;
using BlazorMinimalApis.Pages.Data;
using BlazorMinimalApis.Pages.Lib;
using BlazorMinimalApis.Pages.Pages;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

using static BlazorMinimalApis.Pages.Pages.Login;

namespace BlazorMinimalApis.Endpoints.Pages
{
    public class DatasetPageController : PageController, IRouteDefinition
    {
        private readonly UsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly AuthService _cookieService;

        public DatasetPageController(UsersService usersService,
            IConfiguration configuration,
            AuthService cookieService)
        {
            this._usersService = usersService;
            this._configuration = configuration;
            this._cookieService = cookieService;
        }
        public IResult Edit(int id)
        {
            return Page<DatasetDetails>(id);
        }

        public IResult Login()
        {
            return Page<Login>();
        }

        public void Map(WebApplication app)
        {
            app.MapGet("/dataset/{id:int}", Edit);
            app.MapGet("/login", Login);
            app.MapGet("/register", Register);
            app.MapPost("/register", RegisterUser).WithName("RegisterUser");
        }

        private async Task<IResult> RegisterUser(HttpContext context, [FromForm] RegisterForm form)
        {
            var validation = Validate(form);
            if (validation.HasErrors)
            {
                var model = new { Form = form };
                return Page<Register>(model);
            }
            var existingUser = await _usersService.FindUserByEmailAsync(form.Email);

            if (existingUser != null)
            {
                // show error somehow
                //ModelState.AddModelError("EmailExists", "Email already in use by another account.");
                //return Page();
            }

            var userForm = new User()
            {
                Name = form.Email,
                Email = form.Email,
                Password = _usersService.GetSha256Hash(form.Password),
                CreatedAt = DateTime.UtcNow
            };

            var newUser = await _usersService.CreateUserAsync(userForm);

            // Broadcast user created event. Sends welcome email
            //var userCreated = new UserCreated(newUser);
            //await _dispatcher.Broadcast(userCreated);

            var user = await _usersService.FindUserAsync(newUser.Email, newUser.Password);

            var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
            var cookieClaims = await _cookieService.CreateCookieClaims(user);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
                });
            return Redirect("/");
        }

        public IResult Register()
        { 
            return Page<Register>(new { Form = new RegisterForm { ConfirmPassword = "test", Email = "test@test.com", Password = "test" } }); 
        }

        public async Task<IResult> SignIn(SignInManager<IdentityUser> signInManager, [FromForm] LoginModelForm loginModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect("/");
            }
            else
            {
                // Handle failed login attempt
                return Login();
            }
            
        }
    }

    public class RegisterForm
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }

}
