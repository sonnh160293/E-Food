using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodOnline.Application.Service
{
    public class RoleService : IRoleService
    {
        private RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {

            return await _roleManager.Roles.Where(r => !r.Name.Equals(RoleConstant.SUPERADMIN) && !r.Name.Equals(RoleConstant.CUSTOMER)).ToListAsync();
        }

    }
}
