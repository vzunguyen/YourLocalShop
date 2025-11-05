using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;
using YourLocalShop.Models.ViewModels;

namespace YourLocalShop.Controllers;

public class ProductsController : Controller
{
    private readonly ICatalogueRepository _catalogue;

    public ProductsController(ICatalogueRepository catalogue)
    {
        _catalogue = catalogue;
    }

    // /Products?q=milk&category=Dairy&page=1&pageSize=12
    [HttpGet]
    public IActionResult Index(string? q, string? category, int page = 1, int pageSize = 12)
    {
        var all = _catalogue.Search(q, category);
        var total = all.Count();
        var items = all.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vm = new ProductSearchVm
        {
            Q = q,
            Category = category,
            Categories = _catalogue.GetCategories(),
            Results = items,
            Page = page,
            TotalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize)),
            PageSize = pageSize
        };

        return View(vm);
    }

    public IActionResult Details(int id)
    {
        var p = _catalogue.GetProductById(id);
        return p is null ? NotFound() : View(p);
    }
}