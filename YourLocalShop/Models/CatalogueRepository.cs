namespace YourLocalShop.Models;

public interface ICatalogueRepository
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    IEnumerable<Product> Search(string? q, string? categoryName);
    IEnumerable<string> GetCategories(); // for dropdown
}

public class CatalogueRepository : ICatalogueRepository
{
    private static readonly Catalogue _cat = Catalogue.Instance;

    public IEnumerable<Product> GetAllProducts() =>
        _cat.Products.OrderBy(p => p.Name);

    public Product? GetProductById(int id) =>
        _cat.Products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> Search(string? q, string? categoryName)
    {
        IEnumerable<Product> query = _cat.Products;

        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            var c = categoryName.Trim().ToLower();
            query = query.Where(p => p.CategoryName.ToLower() == c);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(s) ||
                (p.Description != null && p.Description.ToLower().Contains(s)));
        }

        return query.OrderBy(p => p.Name);
    }

    public IEnumerable<string> GetCategories() =>
        _cat.Products.Select(p => p.CategoryName)
            .Distinct()
            .OrderBy(n => n);
}