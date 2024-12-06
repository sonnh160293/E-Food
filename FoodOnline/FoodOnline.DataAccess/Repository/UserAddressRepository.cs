using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class UserAddressRepository : BaseRepository<UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<int> DeleteAddress(UserAddress userAddress)
        {
            try
            {
                MarkAsDelete(userAddress);
                return await Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAddress?> GetAddressAsync(Expression<Func<UserAddress, bool>> expression = null, params Expression<Func<UserAddress, object>>[] includeProperties)
        {
            return await GetSingleAsync(expression, includeProperties);
        }

        public async Task<IEnumerable<UserAddress>> GetAddressesAsync(Expression<Func<UserAddress, bool>> expression = null, params Expression<Func<UserAddress, object>>[] includeProperties)
        {
            return await GetAllAsync(expression, includeProperties);

        }

        public async Task<int> InsertAddress(UserAddress userAddress)
        {
            try
            {
                await Create(userAddress);
                await Commit();
                return userAddress.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAddress(UserAddress userAddress)
        {
            try
            {
                Update(userAddress);
                return await Commit();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
