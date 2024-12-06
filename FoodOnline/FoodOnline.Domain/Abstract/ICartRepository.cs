using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCartsAsync(Expression<Func<Cart, bool>> expression = null, params Expression<Func<Cart, object>>[] includeProperties);
        Task<Cart> GetCartAsync(Expression<Func<Cart, bool>> expression = null, params Expression<Func<Cart, object>>[] includeProperties);

        Task<int> InsertCartsAsync(List<Cart> carts);
        Task<int> UpdateCartsAsync(List<Cart> carts);
        Task<int> DeleteCartsAsync(List<Cart> carts);
        Task<int> InsertCartAsync(Cart cart);
        Task<int> DeleteCartAsync(Cart cart);
        Task<int> UpdateCartAsync(Cart cart);
    }
}
