using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IProductDetailRepository
    {
        Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(Expression<Func<ProductDetail, bool>> expression = null, params Expression<Func<ProductDetail, object>>[] includeProperties);
        Task<ProductDetail> GetProductDetailAsync(Expression<Func<ProductDetail, bool>> expression = null, params Expression<Func<ProductDetail, object>>[] includeProperties);
        Task<int> InsertAsync(ProductDetail productDetail);
        Task<int> UpdateAsync(ProductDetail productDetail);
        Task<int> DeleteAsync(ProductDetail productDetail);
        Task<int> InsertListAsync(List<ProductDetail> productDetailList);
    }
}
