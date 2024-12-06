using FoodOnline.Application.DTOs.PostDTO;

namespace FoodOnline.Infrastructure.IService
{
    public interface IPaymentService
    {
        Task<string> GetPaymentLink(List<OrderDetailPostDTO> orderDetailPostDTOs, long orderCode, int total);
        Task ConfirmWebhook();
    }
}
