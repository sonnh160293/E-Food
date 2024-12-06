using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Domain.Common;

namespace FoodOnline.Application.IService
{
    public interface IOrderService
    {
        //order status
        Task<IEnumerable<OrderStatusDropDownDTO>> GetOrderStatusesAsync();


        //order
        Task<PaginatedList<OrderGetDTO>> GetOrdersByStatusAsync(OrderSeachRequestAdmin? orderSeachRequest, int? branchId);
        Task<PaginatedList<OrderGetDTO>> GetOrdersByUserAsync(OrderSeachRequest orderSeachRequest);
        Task<OrderGetDTO> GetOrderByCode(long code);
        Task<ResponseModel> InsertOrderAsync(OrderPostDTO orderPostDTO);
        Task<ResponseModel> UpdateOrderAsync(OrderUpdateDTO orderUpdateDTO);
        Task<ResponseModel> ChangePaymentStatusAsync(long code, bool paymentStatus);
        Task<ResponseModel> ChangeOrderConfirmStatusAsync(long code, bool confirmStatus);

        //order detail
        Task<IEnumerable<OrderDetailGetDTO>> GetOrderDetailsByOrderId(int orderId);
        Task<ResponseModel> InsertOrderDetails(List<OrderDetailPostDTO> orderDetailPostDTOs);
    }
}
