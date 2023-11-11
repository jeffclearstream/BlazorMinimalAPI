using BlazorMinimalApis.Auth;
using BlazorMinimalApis.Data;
using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMinimalApis.Endpoints.Pages
{
    public partial class Register
    {
        private readonly IConfiguration _configuration;
        private readonly UsersService _usersService;
        private readonly AuthService _cookieService;
        private IDispatcher _dispatcher;

        [BindProperty]
        public RegisterModel Model { get; set; }
        public string ReturnUrl { get; set; }

        public Register(
            IConfiguration configuration,
            UsersService usersService,
            AuthService cookieService,
            IDispatcher dispatcher)
        {
            _configuration = configuration;
            _usersService = usersService;
            _cookieService = cookieService;
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> OnPost()
        {
            await Task.Delay(0);
            return null;
            //if (!ModelState.IsValid)
            //    return Page();

            //if (Model == null)
            //{
            //    return BadRequest("user is not set.");
            //}

            //var existingUser = await _usersService.FindUserByEmailAsync(Model.Email);

            //if (existingUser != null)
            //{
            //    ModelState.AddModelError("EmailExists", "Email already in use by another account.");
            //    return Page();
            //}

            //var userForm = new User()
            //{
            //    Name = Model.Name,
            //    Email = Model.Email,
            //    Password = _usersService.GetSha256Hash(Model.Password),
            //    CreatedAt = DateTime.UtcNow
            //};

            //var newUser = await _usersService.CreateUserAsync(userForm);

            //// Broadcast user created event. Sends welcome email
            //var userCreated = new UserCreated(newUser);
            //await _dispatcher.Broadcast(userCreated);

            //var user = await _usersService.FindUserAsync(newUser.Email, newUser.Password);

            //var cookieExpirationDays = _configuration.GetValue("Spark:Auth:CookieExpirationDays", 5);
            //var cookieClaims = await _cookieService.CreateCookieClaims(user);

            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    cookieClaims,
            //    new AuthenticationProperties
            //    {
            //        IsPersistent = true,
            //        IssuedUtc = DateTimeOffset.UtcNow,
            //        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(cookieExpirationDays)
            //    });

            //return Redirect("~/");
        }

    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
