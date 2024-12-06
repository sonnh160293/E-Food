namespace FoodOnline.Application.DTOs.GetDTO
{
    public class ResponseShippingFeeDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public FeeDTO Fee { get; set; }
    }
}
