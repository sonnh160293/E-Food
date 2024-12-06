using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;

namespace FoodOnline.DataAccess.Repository
{
    public class ProductRelatedRepository : BaseRepository<ProductRelated>, IProductRelatedRepository
    {
        public ProductRelatedRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductRelated>> GetRelatedOfProductAsync(int productId)
        {
            return await GetAllAsync(p => p.ProductId == productId);
        }

        public async Task<int> InsertRelatedProductAsync(int productId, List<int> productsRelatedId)
        {
            var productsRelated = new List<ProductRelated>();
            foreach (var product in productsRelatedId)
            {
                var productRelated = new ProductRelated()
                {
                    ProductId = productId,
                    ProductRelatedId = product
                };
                productsRelated.Add(productRelated);
            }
            await CreateList(productsRelated);
            return await Commit();
        }

        public async Task<int> DeleteRelatedProductAsync(List<ProductRelated> productRelateds)
        {
            DeleteList(productRelateds);
            return await Commit();
        }
    }
}
