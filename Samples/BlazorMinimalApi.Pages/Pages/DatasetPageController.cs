using BlazorMinimalApis.Lib.Routing;
using BlazorMinimalApis.Pages.Lib;
using BlazorMinimalApis.Pages.Pages;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

using static BlazorMinimalApis.Pages.Pages.Login;

namespace BlazorMinimalApis.Endpoints.Pages
{
    public class DatasetPageController : PageController, IRouteDefinition
    {
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

        private IResult RegisterUser(HttpContext context, [FromForm] InputModel registerModel)
        {
            return Redirect("/");
        }

        public IResult Register()
        { 
            return Page<Register>(); 
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

    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }

}
