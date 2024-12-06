using FoodOnline.Application.IService;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FoodOnline.Application.Service
{
    public class CommonService : ICommonService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonService(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetCurrentUser()
        {
            var user = _signInManager.Context?.User;

            if (user.Identity.IsAuthenticated)
            {
                var userClaimName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value ?? string.Empty;
                var userClaimEmail = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value ?? string.Empty;

                return await Task.FromResult(userClaimName + "(" + userClaimEmail + ")");

            }

            return String.Empty;
        }

        public void Set<T>(string key, T value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T Get<T>(string key)
        {
            try
            {
                // Check if HttpContext is available
                if (_httpContextAccessor.HttpContext == null)
                {
                    return default;
                }

                // Get the current HttpContext
                var httpContext = _httpContextAccessor.HttpContext;

                // Check if Session is available
                if (httpContext?.Session == null)
                {
                    return default;
                }

                // Get the value from the session
                var value = httpContext.Session.GetString(key);

                // Return deserialized object or default if value is null
                return value == null ? default : JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception)
            {
                return default;
            }
        }





        public async Task<string?> GetCurrentUserId()
        {
            var user = _signInManager.Context?.User;

            if (user.Identity.IsAuthenticated)
            {
                return await Task.FromResult(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }

            return await Task.FromResult(string.Empty);
        }

        public async Task<string?> GetCurrentUserRole()
        {
            var user = _signInManager.Context?.User;

            if (user.Identity.IsAuthenticated)
            {
                return await Task.FromResult(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);
            }

            return await Task.FromResult(string.Empty);
        }

        public async Task<int?> GetCurrentUserBranch()
        {
            var userId = await GetCurrentUserId();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user == null ? -1 : user.BranchId;
        }
    }
}
