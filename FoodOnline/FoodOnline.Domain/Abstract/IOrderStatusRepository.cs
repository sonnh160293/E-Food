using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IOrderStatusRepository
    {
        Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync(Expression<Func<OrderStatus, bool>> expression = null, params Expression<Func<OrderStatus, object>>[] includeProperties);
    }
}
