using FoodOnline.Application.IService;
using FoodOnline.Application.Service;
using FoodOnline.DataAccess.DataAccess;
using FoodOnline.DataAccess.Repository;
using FoodOnline.Domain.Abstract;
using FoodOnline.Domain.Entities;
using FoodOnline.Domain.IService;
using FoodOnline.Infrastructure.IService;
using FoodOnline.Infrastructure.Service;
using FoodOnline.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOnline.Infrastructure.Configuration
{
    public static class ConfigurationService
    {
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<FoodDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;


                //config password
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;

            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<FoodDbContext>().AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "User";
                options.ExpireTimeSpan = TimeSpan.FromHours(10);

                options.AccessDeniedPath = "";
                options.SlidingExpiration = true;
                options.ReturnUrlParameter = "returnUrl";
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });
        }

        public static void AddDependencyInjection(this IServiceCollection services)
        {


            services.AddScoped<PasswordHasher<ApplicationUser>>();

            services.AddScoped<IBranchRepository, BranchRepository>();

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IProductDetailService, ProductDetailService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserAddressService, UserAddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<CustomCookieAuthenticationEvents>();
            services.AddSingleton<IEmailService, EmailService>();
        }

        public static void AddCustomAutoMapper(this IServiceCollection services)
        {
            // Registers AutoMapper with all the assemblies in the current AppDomain
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }


    }

    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var requestPath = context.Request.Path;

            if (requestPath.StartsWithSegments("/Administration"))
            {
                context.RedirectUri = "/Administration/Authentication/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
            }


            else
            {
                context.RedirectUri = "/Authentication/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
            }

            return base.RedirectToLogin(context);
        }
    }
}
