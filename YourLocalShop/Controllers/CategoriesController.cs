using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;

namespace YourLocalShop.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoriesRepository _categories;

    public CategoriesController(ICategoriesRepository categories)
    {
        _categories = categories;
    }

    // GET: /Categories
    public IActionResult Index()
    {
        var model = _categories.GetAll();
        return View(model);
    }

    // GET: /Categories/Create
    public IActionResult Create()
    {
        return View(new Category());
    }

    // POST: /Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category c)
    {
        if (!ModelState.IsValid) return View(c);
        _categories.Add(c);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Categories/Edit/1
    public IActionResult Edit(int id)
    {
        var c = _categories.GetById(id);
        if (c == null) return NotFound();
        return View(c);
    }

    // POST: /Categories/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category c)
    {
        if (!ModelState.IsValid) return View(c);
        _categories.Update(c);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Categories/Delete/1
    public IActionResult Delete(int id)
    {
        var c = _categories.GetById(id);
        if (c == null) return NotFound();
        return View(c);
    }

    // POST: /Categories/DeleteConfirmed/1
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _categories.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}