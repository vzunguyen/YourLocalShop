using YourLocalShop.Models;
using YourLocalShop.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using YourLocalShop.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// SERVICES
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.Name = ".YourLocalShop.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, ShoppingCartService>();

// REPOSITORIES
builder.Services.AddSingleton<ICatalogueRepository, CatalogueRepository>();

builder.Services.AddSingleton<UsersRepository>();

builder.Services.AddSingleton<OrdersRepository>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/LoginOnly";
        o.LogoutPath = "/Account/Logout";
        o.AccessDeniedPath = "/Account/LoginOnly";
    });

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
// If you’re not using MapStaticAssets for everything, also enable:
app.UseStaticFiles();

app.UseRouting();

// If you add Identity later, place UseAuthentication BEFORE UseAuthorization:
app.UseAuthentication();
// Enable session before auth/authorization/endpoints
app.UseSession();

// app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets();

app.Run();