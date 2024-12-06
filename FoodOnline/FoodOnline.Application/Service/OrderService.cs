using AutoMapper;
using FoodOnline.Application.DTOs;
using FoodOnline.Application.DTOs.GetDTO;
using FoodOnline.Application.DTOs.PostDTO;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Application.Service
{
    public class OrderService : IOrderService
    {

        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, ICommonService commonService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _commonService = commonService;
            _unitOfWork = unitOfWork;
        }



        //order status
        public async Task<IEnumerable<OrderStatusDropDownDTO>> GetOrderStatusesAsync()
        {
            var orderStatuses = await _unitOfWork.OrderStatusRepository.GetOrderStatusesAsync();
            return _mapper.Map<IEnumerable<OrderStatusDropDownDTO>>(orderStatuses);
        }

        //order detail
        public async Task<IEnumerable<OrderDetailGetDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _unitOfWork.OrderDetailRepository.GetOrderDetailsAsync(od => od.OrderId == orderId, od => od.Product.Category);
            return _mapper.Map<IEnumerable<OrderDetailGetDTO>>(orderDetails);
        }

        public async Task<ResponseModel> InsertOrderDetails(List<OrderDetailPostDTO> orderDetailPostDTOs)
        {
            try
            {
                var orderDetails = _mapper.Map<List<OrderDetail>>(orderDetailPostDTOs);
                var result = await _unitOfWork.OrderDetailRepository.InsertListAsync(orderDetails);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Message = status ? "Ok" : "Fail",
                    Status = status
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Status = false,
                    Message = ex.Message
                };
            }
        }


        //order
        public async Task<PaginatedList<OrderGetDTO>> GetOrdersByStatusAsync(OrderSeachRequestAdmin? orderSeachRequest, int? branchId)
        {
            // Build the filter expression
            Expression<Func<Order, bool>> filter = o =>
                (!orderSeachRequest.StatusId.HasValue || o.StatusId == orderSeachRequest.StatusId) &&
                (!orderSeachRequest.IsConfirm.HasValue || o.IsConfirm == orderSeachRequest.IsConfirm) &&
                (!orderSeachRequest.IsPaid.HasValue || o.IsPaid == orderSeachRequest.IsPaid) &&
                (branchId < 0 || o.BranchId == branchId) &&
                (!orderSeachRequest.FromDate.HasValue || o.OrderedDate >= orderSeachRequest.FromDate) &&
                (!orderSeachRequest.ToDate.HasValue || o.OrderedDate <= orderSeachRequest.ToDate);


            var orders = await _unitOfWork.OrderRepository.GetOrdersAsync(filter);
            var orderDTO = _mapper.Map<IEnumerable<OrderGetDTO>>(orders);

            // Ensure PageIndex and PageSize have default values if not set
            int pageIndex = orderSeachRequest?.PageIndex ?? 1;
            int pageSize = orderSeachRequest?.PageSize ?? 10;

            var orderPaging = PaginatedList<OrderGetDTO>.Create((List<OrderGetDTO>)orderDTO, pageIndex, pageSize);
            return orderPaging;
        }


        public async Task<PaginatedList<OrderGetDTO>> GetOrdersByUserAsync(OrderSeachRequest orderSeachRequest)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersAsync(o =>
                                                                          o.StatusId == orderSeachRequest.StatusId &&
                                                                          o.CustomerId.Equals(orderSeachRequest.CustomerId) &&
                                                                         (orderSeachRequest.FromDate == null ||
                                                                         (orderSeachRequest.ToDate == null ? o.OrderedDate >= orderSeachRequest.FromDate && o.OrderedDate <= DateTime.Now :
                                                                             o.OrderedDate >= orderSeachRequest.FromDate && o.OrderedDate <= orderSeachRequest.ToDate)) &&
                                                                         (orderSeachRequest.ToDate == null || (orderSeachRequest.FromDate == null ? o.OrderedDate <= orderSeachRequest.ToDate :
                                                                           o.OrderedDate >= orderSeachRequest.FromDate && o.OrderedDate <= orderSeachRequest.ToDate)), o => o.OrderStatus);
            var orderDTO = _mapper.Map<IEnumerable<OrderGetDTO>>(orders);
            var orderPaging = PaginatedList<OrderGetDTO>.Create((List<OrderGetDTO>)orderDTO, orderSeachRequest.PageIndex, orderSeachRequest.PageSize);
            return orderPaging;
        }


        public async Task<ResponseModel> InsertOrderAsync(OrderPostDTO orderPostDTO)
        {
            try
            {
                orderPostDTO.OrderedDate = DateTime.Now;
                orderPostDTO.CustomerId = await _commonService.GetCurrentUserId();
                orderPostDTO.OrderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var order = _mapper.Map<Order>(orderPostDTO);
                int result = await _unitOfWork.OrderRepository.InsertAsync(order);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Data = result,
                    Status = status,
                    Message = status ? "Ok" : "Fail"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Insert,
                    Status = false,
                    Message = ex.Message
                };
            }
        }



        public async Task<ResponseModel> UpdateOrderAsync(OrderUpdateDTO orderUpdateDTO)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderAsync(o => o.Id == orderUpdateDTO.Id);
                order.StatusId = orderUpdateDTO.StatusId;
                if (orderUpdateDTO.StatusId == OrderStatusConstant.Done)
                {
                    order.IsPaid = true;

                }
                order.LastModifiedBy = await _commonService.GetCurrentUser() ?? String.Empty;
                int result = await _unitOfWork.OrderRepository.UpdateAsync(order);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,

                    Status = status,
                    Message = status ? "Ok" : "Fail"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<OrderGetDTO> GetOrderByCode(long code)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderAsync(o => o.OrderCode == code);
            return _mapper.Map<OrderGetDTO>(order);
        }

        public async Task<ResponseModel> ChangePaymentStatusAsync(long code, bool paymentStatus)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderAsync(o => o.OrderCode == code);
                order.IsPaid = paymentStatus;
                order.IsConfirm = true;
                order.StatusId = OrderStatusConstant.Prepare;
                int result = await _unitOfWork.OrderRepository.UpdateAsync(order);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,

                    Status = status,
                    Message = status ? "Ok" : "Fail"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseModel> ChangeOrderConfirmStatusAsync(long code, bool confirmStatus)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderAsync(o => o.OrderCode == code);
                order.IsConfirm = confirmStatus;
                order.StatusId = OrderStatusConstant.Prepare;

                int result = await _unitOfWork.OrderRepository.UpdateAsync(order);
                var status = result > 0;
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,

                    Status = status,
                    Message = status ? "Ok" : "Fail"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Action = Domain.Enums.ActionType.Update,
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
