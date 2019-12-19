using IdentityServer4;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyApp.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.IdentityServer.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly TestUserStore _userStore;

        public AccountController()
        {
            _userStore = new TestUserStore(Config.GetUsers());
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                Username = "getson",
                Password = "password",
                ReturnUrl = returnUrl
            };
            return View("Views/Login.cshtml", viewModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel viewModel)
        {
            if (!_userStore.ValidateCredentials(viewModel.Username, viewModel.Password))
            {
                ModelState.AddModelError("", "Invalid username or password");
                viewModel.Password = string.Empty;
                return View("Views/Login.cshtml", viewModel);
            }

            // Use an IdentityServer-compatible ClaimsPrincipal
            var identityServerUser = new IdentityServerUser(viewModel.Username);
            identityServerUser.DisplayName = viewModel.Username;
            await HttpContext.SignInAsync("Bearer", identityServerUser.CreatePrincipal());

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identityServerUser.CreatePrincipal());

            return Redirect(viewModel.ReturnUrl);
        }
    }
}
