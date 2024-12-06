using Microsoft.AspNetCore.Identity;

namespace FoodOnline.Application.IService
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
    }
}
