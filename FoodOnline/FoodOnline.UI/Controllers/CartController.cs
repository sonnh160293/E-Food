using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Controllers
{
    public class CartController : Controller
    {

        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICommonService _commonService;

        public CartController(ICartService cartService, IProductService productService, ICommonService commonService)
        {
            _cartService = cartService;
            _productService = productService;
            _commonService = commonService;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _cartService.GetCartsAsync(await _commonService.GetCurrentUserId() ?? string.Empty);
            if (carts.Any(c => string.IsNullOrEmpty(c.CustomerId)))
            {
                foreach (var item in carts)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    item.Product = product;
                }
            }

            ViewData["NumberCart"] = carts.Count();

            return View(carts);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartPostDTO cartPostDTO)
        {
            try
            {
                cartPostDTO.CustomerId = await _commonService.GetCurrentUserId() ?? String.Empty;
                var response = await _cartService.InsertCartAsync(cartPostDTO);

                if (response.Status)
                {
                    return Json(response.Data);
                }
                return Json(-1);
            }
            catch (Exception)
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] List<CartPostDTO> cartsPostDTO)
        {
            try
            {


                var response = await _cartService.UpdateCartsAsync(cartsPostDTO);

                if (response.Status)
                {
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {


                var response = await _cartService.DeleteCartAsync(productId);

                if (response.Status)
                {
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
