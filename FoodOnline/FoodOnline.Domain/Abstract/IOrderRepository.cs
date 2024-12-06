using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IOrderRepository
    {

        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> expression = null, params Expression<Func<Order, object>>[] includeProperties);
        Task<Order> GetOrderAsync(Expression<Func<Order, bool>> expression = null, params Expression<Func<Order, object>>[] includeProperties);
        Task<int> InsertAsync(Order order);
        Task<int> UpdateAsync(Order order);
        Task<int> DeleteAsync(Order order);

    }
}
