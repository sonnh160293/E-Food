using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Areas.Admin.Controllers
{

    [Area("Administration")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }




        // GET: /Admin/Branch/Index
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 2, string? keyword = null, bool? status = null)
        {
            var branchSearchRequest = new BranchSearchRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword,
                Status = status
            };

            var paginatedBranches = await _branchService.GetBranchesAsync(branchSearchRequest);

            // Pass search parameters to ViewData for use in the View
            ViewData["Keyword"] = keyword;
            ViewData["Status"] = status;
            ViewData["PageIndex"] = pageIndex;
            ViewData["PageSize"] = pageSize;

            return View(paginatedBranches);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBranch(BranchPostDTO branchPostDTO)
        {
            if (ModelState.IsValid)
            {
                var branchId = await _branchService.InsertBranchAsync(branchPostDTO);
                if (branchId > 0)
                {
                    return RedirectToAction("Index", "Branch");
                }
                return PartialView("~/Areas/Admin/Views/Branch/_AddModal.cshtml", branchPostDTO);
            }
            return PartialView("~/Areas/Admin/Views/Branch/_AddModal.cshtml", branchPostDTO);
        }
    }

}
