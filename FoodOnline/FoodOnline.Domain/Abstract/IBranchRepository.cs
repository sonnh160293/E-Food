using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetBranchesAsync(Expression<Func<Branch, bool>> expression = null);

        Task<int> InsertBranchAsync(Branch branch);
    }
}
