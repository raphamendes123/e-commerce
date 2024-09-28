using Core.ApiConfigurations;
using Front.MVC.Business.Authentication.Interfaces;
using Front.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Front.MVC.Services.Interfaces;

namespace Front.MVC.Controllers
{
    public class IdentityController : MainControllerMvc
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationBusiness _authenticationBusiness;

        public IdentityController(IAuthenticationService authenticationService, IAuthenticationBusiness authenticationBusiness)
        {
            _authenticationService = authenticationService;
            _authenticationBusiness = authenticationBusiness;
        }

        [HttpGet]
        [Route(template: "register")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [Route(template: "register")]
        public async Task<IActionResult> Register(UserRegisterViewModel viewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(viewModel);

            UserLoginResponseViewModel? response = await _authenticationService.Register(viewModel);

            if (HasErrors(response.ResponseResult))
            {
                return View(viewModel);
            }

            await _authenticationBusiness.LogIn(response);

            return string.IsNullOrEmpty(returnUrl) ? RedirectToAction("Index", "Home") : LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route(template: "login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [Route(template: "login")]
        public async Task<IActionResult> Login(UserLoginViewModel viewModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(viewModel);

            UserLoginResponseViewModel? response = await _authenticationService.Login(viewModel);

            if (HasErrors(response.ResponseResult)) 
            {
                return View(viewModel);
            }

            await _authenticationBusiness.LogIn(response);

            return string.IsNullOrEmpty(returnUrl) ? RedirectToAction("Index", "Home") : LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route(template: "logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationBusiness.Logout();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
