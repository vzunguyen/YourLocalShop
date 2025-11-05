using Microsoft.AspNetCore.Authentication.Cookies;
using YourLocalShop.Models; // <-- make sure this matches your repo namespace

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// REPOSITORIES
builder.Services.AddSingleton<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddSingleton<IProductsRepository, ProductsRepository>();
builder.Services.AddSingleton<ICatalogueRepository, CatalogueRepository>();
builder.Services.AddSingleton<InMemoryUserStore>();

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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    // optional: land on Products list by default
    pattern: "{controller=Products}/{action=Index}/{id?}"
).WithStaticAssets();

app.Run();