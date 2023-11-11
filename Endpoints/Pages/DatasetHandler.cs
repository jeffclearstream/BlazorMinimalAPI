using BlazorMinimalApis.Data;
using BlazorMinimalApis.Endpoints.Pages.Contacts;
using BlazorMinimalApis.Lib.Routing;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

using Org.BouncyCastle.Crypto;

using static BlazorMinimalApis.Endpoints.Pages.Login;

namespace BlazorMinimalApis.Endpoints.Pages
{
    public class DatasetHandler : PageHandler
    {
        public IResult Edit(int id)
        {
            return Page<DatasetDetails>(id);
        }

        public IResult Login()
        {
            return Page<Login>();
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
}
