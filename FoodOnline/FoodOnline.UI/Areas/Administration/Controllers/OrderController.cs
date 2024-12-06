using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.DTOs.ViewModel;
using FoodOnline.Application.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOnline.UI.Areas.Admin.Controllers
{
    [Area("Administration")]
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;
        private readonly ICommonService _commonService;

        public OrderController(IOrderService orderService, ICommonService commonService)
        {
            _orderService = orderService;
            _commonService = commonService;
        }

        public async Task<IActionResult> Index(OrderSeachRequestAdmin searchRequest)
        {
            var userBranch = await _commonService.GetCurrentUserBranch();
            // Initialize searchRequest if it is null
            if (searchRequest == null)
            {
                searchRequest = new OrderSeachRequestAdmin
                {
                    PageIndex = 1, // Default to first page
                    PageSize = 10  // Default page size
                };
            }
            else
            {
                // Ensure PageIndex and PageSize have default values if not set
                searchRequest.PageIndex = searchRequest.PageIndex > 0 ? searchRequest.PageIndex : 1;
                searchRequest.PageSize = searchRequest.PageSize > 0 ? searchRequest.PageSize : 10;
            }

            // Fetch filtered and paginated orders
            var orders = await _orderService.GetOrdersByStatusAsync(searchRequest, userBranch);

            // Fetch order statuses for dropdown
            var statuses = await _orderService.GetOrderStatusesAsync();

            // Create a view model to pass to the view
            var viewModel = new OrderIndexViewModel
            {
                Orders = orders,
                SearchRequest = searchRequest,
                OrderStatuses = statuses,
                PaidOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All" },
            new SelectListItem { Value = "true", Text = "Yes" },
            new SelectListItem { Value = "false", Text = "No" }
        },
                ConfirmOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All" },
            new SelectListItem { Value = "true", Text = "Yes" },
            new SelectListItem { Value = "false", Text = "No" }
        }
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(long orderCode)
        {
            if (orderCode <= 0)
            {
                return BadRequest(new { message = "Invalid order code." });
            }

            var order = await _orderService.GetOrderByCode(orderCode);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            var orderDetails = await _orderService.GetOrderDetailsByOrderId(order.Id);
            var statuses = await _orderService.GetOrderStatusesAsync();

            var orderModel = new OrderDetailViewModel
            {
                Order = new OrderGetDTO
                {
                    Id = order.Id,
                    OrderCode = order.OrderCode,
                    Status = order.Status,
                    IsPaid = order.IsPaid,
                    IsConfirm = order.IsConfirm,
                    IsCOD = order.IsCOD,
                    OrderedDate = order.OrderedDate,
                    StatusId = order.StatusId
                },
                OrderDetails = orderDetails.Select(d => new OrderDetailGetDTO
                {
                    ProductName = d.ProductName,
                    Quantity = d.Quantity
                }).ToList(),
                Statuses = statuses.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromForm] OrderUpdateDTO orderUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, message = "Invalid data." });
            }

            var response = await _orderService.UpdateOrderAsync(orderUpdateDTO);

            if (response.Status)
            {
                return Json(new { status = true, message = "Order updated successfully." });
            }
            else
            {
                return Json(new { status = false, message = response.Message });
            }
        }



    }
}
