using FoodOnline.Application.DTOs.ViewModel;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Areas.Admin.Controllers
{
    [Area("Administration")]
    public class AuthenticationController : Controller
    {

        private readonly IUserService _userService;
        private readonly IAuthenticateService _authenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AuthenticationController(IUserService userService, SignInManager<ApplicationUser> signInManager, IAuthenticateService authenticationService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authenticationService.CheckLogin(model.Email, model.Password, model.HasRemember);
                    if (result.Status)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.Error = result.Message;
                    return View(model);
                }

                catch (NotFoundException ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
                catch (InvalidException ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }

            }

            return View(model);


        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Authentication");
        }
    }
}
