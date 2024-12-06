using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOnline.UI.Areas.Admin.Controllers
{
    [Area("Administration")]
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IBranchService _branchService;
        public AccountController(IUserService userService, IRoleService roleService, IBranchService branchService)
        {
            _roleService = roleService;
            _userService = userService;
            _branchService = branchService;
        }



        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetUserPagination(RequestDatatable requestDatatable)
        {
            var user = await _userService.GetAccountPagination(requestDatatable);
            return Json(user);

        }


        [HttpGet]
        public async Task<IActionResult> Insert()
        {

            await PopulateDropdowns();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Insert(AccountPostDTO accountPostDTO)
        {
            await PopulateDropdowns();

            if (ModelState.IsValid)
            {
                var insertAccount = await _userService.InsertAccount(accountPostDTO);
                if (insertAccount.Status)
                {
                    return RedirectToAction("Index", "Account");
                }
                ViewBag.Errors = insertAccount.Message;

                return View(accountPostDTO);
            }


            return View(accountPostDTO);
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Roles = new SelectList(await _roleService.GetAllRolesAsync(), "Name", "Name");
            ViewBag.Branches = new SelectList(await _branchService.GetBranchForDropDown(), "Id", "Name");
        }
    }
}
