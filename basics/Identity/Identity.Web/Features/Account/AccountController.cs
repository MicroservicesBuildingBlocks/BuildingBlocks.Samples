using System.Security.Claims;
using System.Threading.Tasks;
using BuildingBlocks.AspnetCoreIdentity.RavenDB;
using Identity.Application;
using Identity.Application.CommandSide.Commands;
using Identity.Application.Features.Account;
using Identity.Application.QuerySide.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Features.Home;

namespace Identity.Web.Features.Account
{
    public class AccountController 
        : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(
            IMediator mediator
            )
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _mediator.Send(new LogoffCommand());
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var result = await _mediator.Send(new LogoutInfoQuery(logoutId));
            if (result.ShowLogoutPrompt == false)
            {
                return await Logout(result);
            }
            return View(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInfo model)
        {
            var result = await _mediator.Send(new LoggedOutInfoQuery(model.LogoutId));
            await _mediator.Send(new LogoffCommand());
            return Redirect(result.PostLogoutRedirectUri);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var result = await _mediator.Send(new RegisterCommand(
                model.Name, model.Password));

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToLocal(returnUrl);
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var result = await _mediator.Send(
                new LoginCommand(model.Name, model.Password, model.RememberMe)
            );

            if (result)
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl) => Url.IsLocalUrl(returnUrl)
            ? (IActionResult) Redirect(returnUrl)
            : RedirectToAction(nameof(HomeController.Index), "Home");

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
