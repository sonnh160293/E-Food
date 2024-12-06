using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.DTOs.ViewModel;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using FoodOnline.Infrastructure.IService;
using FoodOnline.UI.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace FoodOnline.UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IUserAddressService _userAddressService;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IShippingService _shippingService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        private readonly IEmailService _emailService;

        private string ClientId;
        private string ApiKey;
        private string ChecksumKey;
        private readonly PayOS _payOS;


        public OrderController(ICommonService commonService, IUserAddressService userAddressService,
            IUserService userService, ICartService cartService,
            IShippingService shippingService, IOrderService orderService,
            IConfiguration configuration, IPaymentService paymentService, IEmailService emailService)
        {
            _commonService = commonService;
            _userAddressService = userAddressService;
            _cartService = cartService;
            _userService = userService;
            _shippingService = shippingService;
            _orderService = orderService;
            _configuration = configuration;
            _paymentService = paymentService;
            _emailService = emailService;

            ClientId = _configuration["PayOS: ClientId"];
            ApiKey = _configuration["PayOS: ApiKey"];
            ChecksumKey = _configuration["PayOS: ChecksumKey"];
            _payOS = new PayOS(ClientId, ApiKey, ChecksumKey);
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(List<int> checkedItemIds)
        {
            var userId = await _commonService.GetCurrentUserId();
            var user = await _userService.GetUserById(userId);
            var userAddress = await _userAddressService.GetDefaultAddressOfUserAsync(userId);
            var orderPostDTO = new OrderPostDTO()
            {
                ReceiverName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                ReceiverAddress = $"{userAddress.Detail}, {userAddress.Street}, {userAddress.District}, {userAddress.City}"
            };

            var address = new UserAddressDetailGetDTO()
            {
                Detail = userAddress.Detail,
                Street = userAddress.Street,
                District = userAddress.District,
                City = userAddress.City,
            };
            var shippingFeeResponse = await _shippingService.GetShippingFeeAsync(address);
            ViewBag.ShippingFee = shippingFeeResponse;

            var selectedTimeSlot = TimeSlotHelper.GenerateTimeSlots();
            ViewBag.SelectedTimeSlot = selectedTimeSlot;

            var cartItems = await _cartService.GetCartItemsByIdsAsync(checkedItemIds);
            ViewBag.SelectedItems = cartItems;

            return View(orderPostDTO);
        }

        [HttpPost]
        [SkipActionFilter]
        public async Task<IActionResult> Checkout(OrderPostDTO orderPostDTO, List<OrderDetailPostDTO> orderDetailPostDTOs)
        {
            if (ModelState.IsValid)
            {

                var orderResponse = await _orderService.InsertOrderAsync(orderPostDTO);
                if (orderResponse.Status)
                {
                    // Insert order details
                    foreach (var item in orderDetailPostDTOs)
                    {
                        item.OrderId = (int?)orderResponse.Data;
                    }
                    var orderDetailResponse = await _orderService.InsertOrderDetails(orderDetailPostDTOs);

                    if (orderDetailResponse.Status)
                    {
                        if ((bool)orderPostDTO.IsCOD)
                        {
                            var subject = "Confirm Your Order";
                            var body = $"<h1>Thank you for your order!</h1><p>Please confirm your order by clicking <a href='http://localhost:5243/order/confirm?orderCode={orderPostDTO.OrderCode}'>here</a>.</p>";
                            await _emailService.SendEmailAsync(orderPostDTO.Email, subject, body);
                            return RedirectToAction("OrderConfirmation", new { email = orderPostDTO.Email });
                        }

                        return await ProcessPayment(orderDetailPostDTOs, orderPostDTO.OrderCode, (int)orderPostDTO.TotalPrice);

                    }
                    ViewBag.Errors = orderDetailResponse.Message;
                    return View(orderPostDTO);
                }
                ViewBag.Errors = orderResponse.Message;
                return View(orderPostDTO);

            }
            return View(orderPostDTO);
        }

        [HttpGet]
        public IActionResult OrderConfirmation(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            // Get the current user ID
            var customerId = await _commonService.GetCurrentUserId();

            // Fetch the orders for the current user
            var orderSeachRequest = new OrderSeachRequest
            {
                CustomerId = customerId,
                PageIndex = 1, // You might want to implement paging
                PageSize = 10  // Adjust page size as needed
            };

            var orders = await _orderService.GetOrdersByUserAsync(orderSeachRequest);

            // Fetch order details for each order
            var ordersWithDetails = new List<OrderDetailViewModel>();

            foreach (var order in orders.Items)
            {
                var orderDetails = await _orderService.GetOrderDetailsByOrderId(order.Id);
                ordersWithDetails.Add(new OrderDetailViewModel
                {
                    Order = order,
                    OrderDetails = orderDetails
                });
            }

            // Pass the data to the view
            return View(ordersWithDetails);
        }

        [AllowAnonymous]
        [HttpPost("UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatus([FromBody] WebhookType webhookBody)
        {
            if (webhookBody.desc.Equals("success"))
            {
                WebhookData webhookData = _payOS.verifyPaymentWebhookData(webhookBody);
                var orderCode = webhookData.orderCode;
                var responseUpdate = await _orderService.ChangePaymentStatusAsync(orderCode, true);
                return Ok();
            }
            return BadRequest("Invalid webhook payload.");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TestOrder()
        {
            return Json(1);
        }

        [SkipActionFilter]
        private async Task<IActionResult> ProcessPayment(List<OrderDetailPostDTO> orderDetailPostDTOs, long orderCode, int totalPrice)
        {
            string urlPayment = await _paymentService.GetPaymentLink(orderDetailPostDTOs, orderCode, totalPrice);

            return Redirect(urlPayment);
        }


        [HttpGet]
        public async Task<IActionResult> Success(long? orderCode)
        {
            if (orderCode.HasValue)
            {
                var responseUpdate = await _orderService.ChangePaymentStatusAsync((long)orderCode, true);
                if (responseUpdate.Status)
                {
                    ViewBag.OrderCode = orderCode;
                    return View();
                }
            }
            return RedirectToAction("Index", "Product");
        }


        [HttpGet]
        public async Task<IActionResult> Confirm(long orderCode)
        {
            var responseUpdate = await _orderService.ChangeOrderConfirmStatusAsync(orderCode, true);
            ViewBag.OrderCode = orderCode;
            return View();
        }
    }
}
