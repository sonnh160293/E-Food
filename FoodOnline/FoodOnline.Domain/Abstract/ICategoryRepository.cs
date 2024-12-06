using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(Expression<Func<Category, bool>> expression = null);
        Task<Category> GetCategoryAsync(Expression<Func<Category, bool>> expression = null);
        Task<int> InsertAsync(Category category);
        Task<int> UpdateAsync(Category category);
        Task<int> DeleteAsync(Category category);
    }
}