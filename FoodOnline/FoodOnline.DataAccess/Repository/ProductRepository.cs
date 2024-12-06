using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteAsync(Product product)
        {
            try
            {
                MarkAsDelete(product);
                int result = await Commit();
                return result > 0 ? product.Id : result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product?> GetProductAsync(Expression<Func<Product, bool>> expression = null, params Expression<Func<Product, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }



        public async Task<IEnumerable<Product>> GetProductsAsync(Expression<Func<Product, bool>> expression = null, params Expression<Func<Product, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }



        public async Task<int> InsertAsync(Product product)
        {
            try
            {
                await Create(product);
                int result = await Commit();
                return result > 0 ? product.Id : result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Product product)
        {
            try
            {
                Update(product);
                int result = await Commit();
                return result > 0 ? product.Id : result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
