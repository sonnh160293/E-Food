using FoodOnline.DataAccess;
using FoodOnline.Infrastructure.Configuration;
using FoodOnline.Infrastructure.IService;
using FoodOnline.UI.Ultility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddTransient<SiteAreaConvention>();

// Add controllers with views and configure conventions
builder.Services.AddControllersWithViews(
//    options =>
//{
//    // Resolve the SiteAreaConvention from the service container
//    var serviceProvider = builder.Services.BuildServiceProvider();
//    var siteAreaConvention = serviceProvider.GetRequiredService<SiteAreaConvention>();

//    // Add the SiteAreaConvention to the conventions
//    options.Conventions.Add(siteAreaConvention);
//}
);
builder.Services.AddCustomAutoMapper();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true; // Session chỉ được truy cập thông qua HTTP
    options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
});

builder.Services.AddHttpClient();



var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
    await paymentService.ConfirmWebhook();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

//seed data
app.AutoMigration().GetAwaiter().GetResult();
app.SeedData(builder.Configuration).GetAwaiter().GetResult();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapAreaControllerRoute(
    name: "AdminRouting",
    areaName: "Administration",
    pattern: "Administration/{controller=Home}/{action=Index}/{id?}"
    );



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
