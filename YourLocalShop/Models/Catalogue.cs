namespace YourLocalShop.Models;

public class Catalogue
{
    public int Id { get; set; }
    public string Name { get; set; } = "Main Catalogue";

    public List<Category> Categories { get; set; } = new();

    public void AddCategory(Category category) => Categories.Add(category);

    public Category? GetCategory(int categoryId) =>
        Categories.FirstOrDefault(c => c.Id == categoryId);

    public IEnumerable<Product> GetAllProducts() =>
        Categories.SelectMany(c => c.Products);

    public IEnumerable<Product> GetProductsByCategory(int categoryId) =>
        GetCategory(categoryId)?.Products ?? Enumerable.Empty<Product>();

    public IEnumerable<Product> SearchProducts(string keyword) =>
        Categories
            .SelectMany(c => c.Products)
            .Where(p =>
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(p.Description) &&
                p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
}
