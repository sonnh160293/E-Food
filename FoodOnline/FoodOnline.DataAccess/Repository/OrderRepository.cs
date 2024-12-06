using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(FoodDbContext context) : base(context)
        {
        }

        public Task<int> DeleteAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> expression = null, params Expression<Func<Order, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> expression = null, params Expression<Func<Order, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }

        public async Task<int> InsertAsync(Order order)
        {
            try
            {
                await Create(order);
                return await Commit() > 0 ? order.Id : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Order order)
        {

            try
            {
                Update(order);
                return await Commit() > 0 ? order.Id : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
