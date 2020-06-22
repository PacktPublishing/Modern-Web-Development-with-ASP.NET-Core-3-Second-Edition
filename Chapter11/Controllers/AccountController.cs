using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chapter07.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chapter07.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //note: if not using identity, remove its services from the constructor
        public AccountController(ApplicationDbContext ctx, ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this._ctx = ctx;
            this._logger = logger;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PerformCreate(string username, string password, string confirmationPassword)
        {
            if (password == confirmationPassword)
            {
                var user = new ApplicationUser();

                await this.TryUpdateModelAsync(user);

                var result = await this._userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await this._userManager.AddToRoleAsync(user, "Admin");
                }

                if (!result.Succeeded)
                {
                    var errors = string.Join('\n', result.Errors.Select(e => e.Description));
                    this.ModelState.AddModelError("User", $"Error creating the user: {errors}");

                    return this.View("Create");
                }
            }

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return this.View(new LoginModel { ReturnUrl = this.HttpContext.Request.Query["ReturnUrl"] });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PerformLogin(string username, string password, string returnUrl, bool isPersistent)
        {
            if (await IsValidCredentials(username, password, isPersistent))
            {
                //dummy
                var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "Admin") }, CookieAuthenticationDefaults.AuthenticationScheme));
                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties { IsPersistent = isPersistent });

                //identity
                //await this._signInManager.SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent });
                
                return this.LocalRedirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(nameof(Forbidden));
            }
        }

        private async Task<bool> IsValidCredentials(string username, string password, bool isPersistent)
        {
            //dummy
            return true;
            //identity
            //var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent, false);
            //return result.Succeeded;
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //dummy
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity
            //await this._signInManager.SignOutAsync();
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
