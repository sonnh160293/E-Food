using FoodOnline.Domain.Entities;

namespace FoodOnline.Domain.Abstract
{
    public interface IProductRelatedRepository
    {

        Task<IEnumerable<ProductRelated>> GetRelatedOfProductAsync(int productId);
        Task<int> InsertRelatedProductAsync(int productId, List<int> productsRelatedId);
        Task<int> DeleteRelatedProductAsync(List<ProductRelated> productRelateds);
    }
}
