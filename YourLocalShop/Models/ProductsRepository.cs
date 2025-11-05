using System.Collections.Concurrent;

namespace YourLocalShop.Models;

public interface IProductsRepository
{
    IQueryable<Product> Query();
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product p);
    void Update(Product p);
    bool Delete(int id);
}

public class ProductsRepository : IProductsRepository
{
    private readonly ConcurrentDictionary<int, Product> _store = new();
    private int _nextId = 1;

    public ProductsRepository()
    {
        // Dairy
        Add(new Product { Name = "Butter 250g", Price = 4.00m, StockQty = 100, CategoryId = 5, Description = "Unsalted Butter" });
        Add(new Product { Name = "Yoghurt 1kg", Price = 7.00m, StockQty = 100, CategoryId = 5, Description = "Plain Greek Yogurt" });
        Add(new Product { Name = "Milk 2L", Price = 3.20m, StockQty = 100, CategoryId = 5, Description = "Full Cream Milk" });
        Add(new Product { Name = "Sliced Cheese 250g", Price = 5.50m, StockQty = 100, CategoryId = 5, Description = "Tasty Cheese 24 slices" });
        // Pantry
            Add(new Product { Name = "Corn Flakes 380g", Price = 7.00m, StockQty = 100, CategoryId = 1, Description = "Crunchy Nut Corn Flakes Breakfast Cereal" });
            Add(new Product { Name = "Instant Noodle 600g", Price = 8.00m, StockQty = 100, CategoryId = 1, Description = "Spicy flavoured Instant Noodle 5 Packs" });
            Add(new Product { Name = "Honey 500g", Price = 9.00m, StockQty = 100, CategoryId = 1, Description = "Pure Honey 100% Australian Honey" });
            Add(new Product { Name = "Peanut Butter 375g", Price = 6.50m, StockQty = 100, CategoryId = 1, Description = "Smooth Natural Peanut Butter" });
            Add(new Product { Name = "Pasta Fusilli 500g", Price = 2.50m, StockQty = 100, CategoryId = 1, Description = "Durum Wheat Pasta" });
            Add(new Product { Name = "BBQ Sauce 500ml", Price = 4.20m, StockQty = 100, CategoryId = 1, Description = "Reduced Salt and Sugar BBQ Sauce" });

            // Cleaning
            Add(new Product { Name = "Toilet Paper 8 Packs", Price = 8.00m, StockQty = 100, CategoryId = 2, Description = "100% Recycled Toilet Paper" });
            Add(new Product { Name = "Laundry Powder 2kg", Price = 30.00m, StockQty = 100, CategoryId = 2, Description = "For Front and Top Washing Machines" });
            Add(new Product { Name = "Dishwashing Liquid 800ml", Price = 9.00m, StockQty = 100, CategoryId = 2, Description = "Fresh Lemon Dishwashing Liquid" });
            Add(new Product { Name = "Glass Cleaner Spray 500ml", Price = 5.00m, StockQty = 100, CategoryId = 2, Description = "Ammonia Free" });

            // Drinks
            Add(new Product { Name = "Tropical Juice 2L", Price = 4.80m, StockQty = 100, CategoryId = 3, Description = "Apple, Pear and Pineapple Juice" });
            Add(new Product { Name = "Coke 1.25L", Price = 4.00m, StockQty = 100, CategoryId = 3, Description = "Zero Sugar Soft Drink" });
            Add(new Product { Name = "Sparkling Water 1.25L", Price = 3.50m, StockQty = 100, CategoryId = 3, Description = "Lightly Sparkling Water" });
            Add(new Product { Name = "Ginger Beer 10 Packs", Price = 17.50m, StockQty = 100, CategoryId = 3, Description = "375ml x 10 Packs" });

            // Snacks
            Add(new Product { Name = "Chocolate Block 180g", Price = 7.50m, StockQty = 100, CategoryId = 4, Description = "60% Dark Chocolate" });
            Add(new Product { Name = "Potato Chips 165g", Price = 6.00m, StockQty = 100, CategoryId = 4, Description = "Ridge Cut Potato Chips" });
            Add(new Product { Name = "Biscuits 250g", Price = 2.50m, StockQty = 100, CategoryId = 4, Description = "Milk Arrowroot Biscuits" });
            Add(new Product { Name = "Cream Wafers 125g", Price = 1.25m, StockQty = 100, CategoryId = 4, Description = "Strawberry Cream Wafers" });
    }

    public IQueryable<Product> Query() => _store.Values.AsQueryable();
    public IEnumerable<Product> GetAll() => _store.Values.OrderBy(p => p.Name);
    public Product? GetById(int id) => _store.TryGetValue(id, out var p) ? p : null;

    public Product Add(Product p)
    {
        p.Id = Interlocked.Increment(ref _nextId);
        _store[p.Id] = p;
        return p;
    }

    public void Update(Product p)
    {
        if (!_store.ContainsKey(p.Id)) throw new ArgumentException("Product not found");
        _store[p.Id] = p;
    }

    public bool Delete(int id) => _store.TryRemove(id, out _);
}