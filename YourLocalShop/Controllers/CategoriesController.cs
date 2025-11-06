using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;

namespace YourLocalShop.Controllers;

public class CategoriesController : Controller
{
    private readonly ICatalogueRepository _catalogue;

    public CategoriesController(ICatalogueRepository catalogue)
    {
        _catalogue = catalogue;
    }

    // GET: /Categories
    public IActionResult Index()
    {
        var model = _catalogue.GetCategories();
        return View(model);
    }
}