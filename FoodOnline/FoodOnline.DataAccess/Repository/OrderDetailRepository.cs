using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FoodDbContext context) : base(context)
        {
        }

        public Task<int> DeleteAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetail> GetOrderDetailAsync(Expression<Func<OrderDetail, bool>> expression = null, params Expression<Func<OrderDetail, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync(Expression<Func<OrderDetail, bool>> expression = null, params Expression<Func<OrderDetail, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }

        public Task<int> InsertAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertListAsync(List<OrderDetail> orderDetailList)
        {
            try
            {
                await CreateList(orderDetailList);
                int result = await Commit();
                return result > 0 ? result : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int> UpdateAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}
