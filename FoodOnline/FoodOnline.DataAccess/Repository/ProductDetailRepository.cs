using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class ProductDetailRepository : BaseRepository<ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteAsync(ProductDetail productDetail)
        {
            try
            {
                MarkAsDelete(productDetail);
                int result = await Commit();
                return result > 0 ? productDetail.Id : result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDetail?> GetProductDetailAsync(Expression<Func<ProductDetail, bool>> expression = null, params Expression<Func<ProductDetail, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(Expression<Func<ProductDetail, bool>> expression = null, params Expression<Func<ProductDetail, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);
        }

        public async Task<int> InsertAsync(ProductDetail productDetail)
        {
            try
            {
                await Create(productDetail);
                int result = await Commit();
                return result > 0 ? productDetail.Id : result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> InsertListAsync(List<ProductDetail> productDetailList)
        {
            try
            {
                await CreateList(productDetailList);
                int result = await Commit();
                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(ProductDetail productDetail)
        {
            try
            {
                Update(productDetail);
                int result = await Commit();
                return result > 0 ? productDetail.Id : result;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
