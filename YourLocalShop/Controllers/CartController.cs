using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;
using YourLocalShop.Services;

namespace YourLocalShop.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cart;
    private readonly ICatalogueRepository _catalogue;

    public CartController(ICartService cart, ICatalogueRepository catalogue)
    {
        _cart = cart;
        _catalogue = catalogue;
    }

    // GET: /Cart
    [HttpGet]
    public IActionResult Index()
    {
        var model = _cart.Get();
        return View(model); // expects 'Views/Cart/Index.cshtml'
    }

    // POST: /Cart/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int productId, int quantity = 1)
    {
        var p = _catalogue.GetProductById(productId);
        if (p is null) return NotFound();

        _cart.Add(p.Id, p.Name, p.Price, quantity);
        return RedirectToAction(nameof(Index));
    }

    // POST: /Cart/Update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int productId, int quantity)
    {
        _cart.UpdateQuantity(productId, quantity);
        return RedirectToAction(nameof(Index));
    }

    // POST: /Cart/Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        _cart.Remove(productId);
        return RedirectToAction(nameof(Index));
    }

    // POST: /Cart/Clear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Clear()
    {
        _cart.Clear();
        return RedirectToAction(nameof(Index));
    }
}