using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(Expression<Func<OrderDetail, bool>> expression = null, params Expression<Func<OrderDetail, object>>[] includeProperties);
        Task<OrderDetail> GetOrderDetailAsync(Expression<Func<OrderDetail, bool>> expression = null, params Expression<Func<OrderDetail, object>>[] includeProperties);
        Task<int> InsertAsync(OrderDetail orderDetail);
        Task<int> UpdateAsync(OrderDetail orderDetail);
        Task<int> DeleteAsync(OrderDetail orderDetail);
        Task<int> InsertListAsync(List<OrderDetail> orderDetailList);
    }
}
