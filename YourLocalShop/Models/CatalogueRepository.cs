using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace YourLocalShop.Models;

public interface ICatalogueRepository
{
    // Products
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    IEnumerable<Product> Search(string? q, int? categoryId);
    
    
    // Categories
    IEnumerable<Category> GetCategories();
    Category? GetCategoryById(int id);
}

public class CatalogueRepository : ICatalogueRepository
{
    private readonly string _categoriesPath = "Data/categories.json";
    private readonly string _productsPath = "Data/products.json";
    
    private readonly List<Category> _categories;
    private readonly List<Product> _products;

    public CatalogueRepository()
    {
        // Load categories
        if (File.Exists(_categoriesPath))
        {
            var json = File.ReadAllText(_categoriesPath);
            _categories = JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
        }
        else
        {
            _categories = new List<Category>();
        }

        // Load products
        if (File.Exists(_productsPath))
        {
            var json = File.ReadAllText(_productsPath);
            _products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
        else
        {
            _products = new List<Product>();
        }

        // Wire up navigation properties
        foreach (var product in _products)
        {
            product.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
        }
    }

    // ---------------- Products ----------------
    public IEnumerable<Product> GetAllProducts() => _products;

    public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);
    
    public IEnumerable<Product> Search(string? q, int? categoryId)
    {
        var query = _products.AsEnumerable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                (!string.IsNullOrEmpty(p.Description) &&
                 p.Description.ToLower().Contains(s)));
        }

        return query.OrderBy(p => p.Name);
    }

    // ---------------- Categories ----------------
    public IEnumerable<Category> GetCategories() => _categories;
    
    public Category? GetCategoryById(int id) => _categories.FirstOrDefault(c => c.Id == id);
}
