using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Entities;
using FoodOnline.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOnline.DataAccess
{
    public static class ConfigurationDB
    {
        public static async Task AutoMigration(this WebApplication webApplication)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var appContext = scope.ServiceProvider.GetRequiredService<FoodDbContext>();
                await appContext.Database.MigrateAsync();
            }


        }

        public static async Task SeedData(this WebApplication webApplication, IConfiguration configuration)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userDefault = configuration.GetSection("DefaultUser")?.Get<DefaultUser>() ?? new DefaultUser();
                var roleDefault = configuration.GetValue<string>("DefaultRole") ?? "SuperAdmin";

                try
                {
                    if (!await roleManager.RoleExistsAsync(roleDefault))
                    {
                        await roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = roleDefault
                        });
                    }

                    if (await userManager.FindByEmailAsync(userDefault.Username) == null)
                    {
                        var user = new ApplicationUser()
                        {
                            Email = userDefault.Username,
                            IsActive = true,
                            AccessFailedCount = 0,
                            UserName = "admin"
                        };

                        var identityUser = await userManager.CreateAsync(user, userDefault.Password);

                        if (identityUser.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, roleDefault);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }


            }

        }
    }
}
