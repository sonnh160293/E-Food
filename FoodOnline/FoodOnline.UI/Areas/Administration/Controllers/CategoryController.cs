using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Areas.Admin.Controllers
{
    [Area("Administration")]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCategoryPagination(RequestDatatable requestDatatable)
        {
            var result = await _categoryService.GetCategoriesPaginationAsync(requestDatatable);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(CategoryPostDTO categoryPostDTO)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                if (categoryPostDTO.Id != 0 && categoryPostDTO.Id != 0)
                {
                    var resultUpdate = await _categoryService.UpdateCategoryAsync(categoryPostDTO);
                    if (resultUpdate.Status)
                    {
                        return Json(new { success = true, data = resultUpdate.Data });
                    }
                }
                else
                {
                    var resultInsert = await _categoryService.InsertCategoryAsync(categoryPostDTO);
                    if (resultInsert.Status)
                    {
                        return Json(new { success = true, data = resultInsert.Data });
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage)
                                         .ToList();

            return Json(new { success = false, errors });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            return Json(result.Status);
        }


    }
}
