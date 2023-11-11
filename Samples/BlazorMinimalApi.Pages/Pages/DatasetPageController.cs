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

        private IResult RegisterUser([FromForm] RegisterForm form)
        {
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
