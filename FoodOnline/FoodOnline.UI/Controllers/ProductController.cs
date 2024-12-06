using FoodOnline.Application.DTOs;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }




        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoryForDropDown();
            ViewBag.Categories = categories;
            ViewBag.DefaultCategoryId = categories.FirstOrDefault()?.Id ?? 2; // Set the default category ID, or 0 if no categories

            var categorySearchRequest = new CategorySearchRequest
            {
                CategoryId = ViewBag.DefaultCategoryId,
                PageIndex = 1, // Start with the first page
                PageSize = 6  // Adjust the page size as needed
            };

            var products = await _productService.GetProductsPagination(categorySearchRequest);
            ViewBag.Products = products;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> TestProduct()
        {
            return Json(1);
        }

        public async Task<IActionResult> FilterProducts(CategorySearchRequest categorySearchRequest)
        {
            categorySearchRequest.PageSize = 6;
            var paginatedProducts = await _productService.GetProductsPagination(categorySearchRequest);

            // Return a PartialView with the correct model type
            return PartialView("_ProductListPartial", paginatedProducts);
        }


    }
}
