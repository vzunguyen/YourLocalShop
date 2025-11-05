using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;
using YourLocalShop.Models.ViewModels;

namespace YourLocalShop.Controllers;

public class ProductsController : Controller
{
    private readonly IProductsRepository _products;
    private readonly ICategoriesRepository _categories;

    public ProductsController(IProductsRepository products, ICategoriesRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    // /Products?q=milk&categoryId=1&page=1
    [HttpGet]
    public IActionResult Index(string? q, int? categoryId, int page = 1, int pageSize = 12)
    {
        var query = _products.Query();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                (p.Description != null && p.Description.ToLower().Contains(s)));
        }

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        var total = query.Count();
        var items = query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vm = new ProductSearchVm
        {
            Q = q,
            CategoryId = categoryId,
            Categories = _categories.GetAll().OrderBy(c => c.Name),
            Results = items,
            Page = page,
            PageSize = pageSize,
            TotalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize))
        };

        return View(vm);
    }
}
