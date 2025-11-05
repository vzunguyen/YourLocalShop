using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;

namespace YourLocalShop.Controllers;

public class CatalogueController : Controller
{
    private readonly ICatalogueRepository _catalogue;

    public CatalogueController(ICatalogueRepository catalogue)
    {
        _catalogue = catalogue;
    }

    // GET: /Catalogue
    public IActionResult Index()
    {
        var model = _catalogue.GetCategories();
        return View(model); // expects Views/Catalogue/Index.cshtml
    }

    // Get: /Catalogue/Details/5
    public IActionResult Details(int id)
    {
        var product = _catalogue.GetProductById(id);
        if (product == null) return NotFound();
        return View(product); // expects Views/Catalogue/Details.cshtml
    }

    // GET: /Catalogue/Search?q=milk&categoryId=1
    public IActionResult Search(string? q, int? categoryId)
    {
        var results = _catalogue.Search(q, categoryId);
        ViewBag.Query = q;
        ViewBag.CategoryId = categoryId;
        ViewBag.Categories = _catalogue.GetCategories();
        return View(results); // expects Views/Catalogue/Search.cshtml
    }
}