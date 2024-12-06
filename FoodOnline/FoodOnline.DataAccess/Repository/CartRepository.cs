using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteCartAsync(Cart cart)
        {
            try
            {
                Delete(cart);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteCartsAsync(List<Cart> carts)
        {
            try
            {
                DeleteList(carts);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cart?> GetCartAsync(Expression<Func<Cart, bool>> expression = null, params Expression<Func<Cart, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }

        public async Task<IEnumerable<Cart>> GetCartsAsync(Expression<Func<Cart, bool>> expression = null, params Expression<Func<Cart, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }

        public async Task<int> InsertCartAsync(Cart cart)
        {
            try
            {
                await Create(cart);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertCartsAsync(List<Cart> carts)
        {
            try
            {
                await CreateList(carts);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateCartAsync(Cart cart)
        {
            try
            {
                Update(cart);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateCartsAsync(List<Cart> carts)
        {
            try
            {
                UpdateList(carts);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
