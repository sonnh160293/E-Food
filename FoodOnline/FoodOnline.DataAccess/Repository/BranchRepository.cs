using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Branch>> GetBranchesAsync(Expression<Func<Branch, bool>> expression = null)
        {
            return await GetAllAsync(expression);
        }

        public async Task<int> InsertBranchAsync(Branch branch)
        {
            try
            {
                await Create(branch);
                if (await Commit() > 0)
                {
                    return branch.Id;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
