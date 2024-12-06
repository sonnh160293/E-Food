using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class OrderStatusRepository : BaseRepository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync(Expression<Func<OrderStatus, bool>> expression = null, params Expression<Func<OrderStatus, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }
    }
}
