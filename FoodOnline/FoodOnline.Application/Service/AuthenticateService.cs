using FoodOnline.Application.DTOs;
using FoodOnline.Application.IService;
using FoodOnline.Domain.Common;
using FoodOnline.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodOnline.Application.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel> CheckLogin(string email, string password, bool hasRemember)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // Check if user exists
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            // Check password
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                // Increase number of failed login attempts
                await _userManager.AccessFailedAsync(user);
                throw new InvalidException("Password is incorrect");
            }

            // Check if account is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                var remainingLockout = user.LockoutEnd.Value - DateTimeOffset.Now;
                return new ResponseModel()
                {
                    Status = false,
                    Message = $"Account is locked out. Please try again in {Math.Round(remainingLockout.TotalMinutes)} minutes"
                };
            }

            // If the login is successful, reset failed access attempts
            await _userManager.ResetAccessFailedCountAsync(user);

            try
            {

                // Create claims
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                // Add other claims as needed
                };

                // Add roles claims
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Create claims identity
                var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

                // Sign in with claims
                await _signInManager.SignInWithClaimsAsync(user, hasRemember, claims);


                return new ResponseModel()
                {
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = false
                };
            }

        }
    }
}
