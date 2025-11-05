namespace YourLocalShop.Models;

public interface ICatalogueRepository
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    IEnumerable<Product> Search(string? q, int? categoryId);
    IEnumerable<Category> GetCategories();
}

public class CatalogueRepository : ICatalogueRepository
{
    private readonly Catalogue _catalogue = new Catalogue();

    public CatalogueRepository(ICategoriesRepository categoriesRepo)
    {
        // Build catalogue from categories repository
        foreach (var category in categoriesRepo.GetAll())
        {
            // Assign Category to each Product in the catalogue
            foreach (var product in category.Products)
            {
                product.Category = category;  // this is to set the navigation property
            }
            
            _catalogue.Categories.Add(category);
        }
    }

    public IEnumerable<Product> GetAllProducts() =>
        _catalogue.GetAllProducts();

    public Product? GetProductById(int id) =>
        _catalogue.GetAllProducts().FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> Search(string? q, int? categoryId)
    {
        var query = _catalogue.GetAllProducts();

        if (categoryId.HasValue)
            query = _catalogue.GetProductsByCategory(categoryId.Value);

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

    public IEnumerable<Category> GetCategories() =>
        _catalogue.Categories;
}
