using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(Expression<Func<Product, bool>> expression = null, params Expression<Func<Product, object>>[] includeProperties);
        Task<Product?> GetProductAsync(Expression<Func<Product, bool>> expression = null, params Expression<Func<Product, object>>[] includeProperties);
        Task<int> InsertAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(Product product);
    }
}
