using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Infrastructure.IService;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;

namespace FoodOnline.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private string ClientId;
        private string ApiKey;
        private string ChecksumKey;
        private readonly PayOS _payOS;

        public PaymentService(IConfiguration configuration, IProductService productService)
        {
            _configuration = configuration;
            _productService = productService;
            ClientId = _configuration["PayOS:ClientId"];
            ApiKey = _configuration["PayOS:ApiKey"];
            ChecksumKey = _configuration["PayOS:ChecksumKey"];
            _payOS = new PayOS(ClientId, ApiKey, ChecksumKey);
        }

        public async Task<string> GetPaymentLink(List<OrderDetailPostDTO> orderDetailPostDTOs, long orderCode, int total)
        {
            var itemsData = await CreateItemsData(orderDetailPostDTOs);
            PaymentData paymentData = new PaymentData(orderCode, total, "Thanh toan don hang",
                                                      itemsData, "http://localhost:5243/Product", "http://localhost:5243/Order/Success");

            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }

        private async Task<List<ItemData>> CreateItemsData(List<OrderDetailPostDTO> orderDetailPostDTOs)
        {
            List<ItemData> items = new List<ItemData>();
            foreach (OrderDetailPostDTO orderDetailPostDTO in orderDetailPostDTOs)
            {
                var itemData = new ItemData(await _productService.GetProductName((int)orderDetailPostDTO.ProductId), (int)orderDetailPostDTO.Quantity, (int)orderDetailPostDTO.UnitPrice);
                items.Add(itemData);

            }
            return items;
        }

        public async Task ConfirmWebhook()
        {
            try
            {
                await _payOS.confirmWebhook("https://c9d2-222-254-23-162.ngrok-free.app/Product/TestProduct");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
