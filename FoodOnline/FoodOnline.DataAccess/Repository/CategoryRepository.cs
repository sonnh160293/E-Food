using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteAsync(Category category)
        {
            try
            {
                MarkAsDelete(category);
                int result = await Commit();
                return result > 0 ? category.Id : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> expression = null)
        {
            return await GetAllAsync(expression);
        }

        public async Task<Category?> GetCategoryAsync(Expression<Func<Category, bool>> expression = null)
        {
            return await GetSingleAsync(expression);
        }



        public async Task<int> InsertAsync(Category category)
        {
            try
            {
                await Create(category);
                int result = await Commit();
                return result > 0 ? category.Id : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Category category)
        {
            try
            {
                Update(category);
                int result = await Commit();
                return result > 0 ? category.Id : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
