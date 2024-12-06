using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.DTOs.ViewModel;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using FoodOnline.UI.Ultility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Controllers
{
    [SkipActionFilter]
    public class AuthenticationController : Controller
    {

        private readonly IUserService _userService;
        private readonly IAuthenticateService _authenticationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserAddressService _userAddressService;


        public AuthenticationController(IUserService userService, SignInManager<ApplicationUser> signInManager, IAuthenticateService authenticationService, IUserAddressService userAddressService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _userAddressService = userAddressService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var loginModel = new LoginViewModel();
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ModelState.Remove("ReturnUrl");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _authenticationService.CheckLogin(model.Email, model.Password, model.HasRemember);
                    if (result.Status)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl); // Redirect đến URL trước đó
                        }
                        return RedirectToAction("Index", "Product");
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

        [HttpGet]
        public async Task<IActionResult> Register()
        {

            var registerModel = new RegisterViewModel();
            return View(registerModel);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var account = new AccountPostDTO()
                {
                    Email = registerViewModel.Email,
                    FullName = registerViewModel.FullName,
                    Password = registerViewModel.Password,
                    Phone = registerViewModel.Phone,
                    RoleName = RoleConstant.CUSTOMER,
                    UserName = registerViewModel.UserName,
                    IsActive = true
                };

                var registerResponse = await _userService.InsertAccount(account);
                if (registerResponse.Status)
                {
                    var accountId = registerResponse.Data.ToString();
                    var address = new UserAddressPostDTO()
                    {
                        IsDefault = true,
                        City = registerViewModel.City,
                        District = registerViewModel.District,
                        Ward = registerViewModel.Ward,
                        Detail = registerViewModel.Detail,
                        UserId = accountId
                    };
                    var addressResponse = await _userAddressService.InsertAddressAsync(address);
                    return RedirectToAction("Login", "Authentication");
                }
                ViewBag.Errors = registerResponse.Message;
                return View(registerViewModel);
            }

            return View(registerViewModel);

        }
    }
}
