using FoodOnline.Domain.Entities;
using System.Linq.Expressions;

namespace FoodOnline.Domain.Abstract
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<UserAddress>> GetAddressesAsync(Expression<Func<UserAddress, bool>> expression = null, params Expression<Func<UserAddress, object>>[] includeProperties);
        Task<UserAddress> GetAddressAsync(Expression<Func<UserAddress, bool>> expression = null, params Expression<Func<UserAddress, object>>[] includeProperties);
        Task<int> InsertAddress(UserAddress userAddress);
        Task<int> UpdateAddress(UserAddress userAddress);
        Task<int> DeleteAddress(UserAddress userAddress);
    }
}
