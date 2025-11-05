using System.Collections.Concurrent;

namespace YourLocalShop.Models;

public interface ICategoriesRepository
{
    IEnumerable<Category> GetAll();
    Category? GetById(int id);
    Category Add(Category c);
    void Update(Category c);
    bool Delete(int id);
}

public class CategoriesRepository : ICategoriesRepository
{
    private readonly ConcurrentDictionary<int, Category> _store = new();
    private int _nextId = 0;
    private int _nextProductId = 0;

    public CategoriesRepository()
    {
        Seed();
    }

    private void Seed()
    {
        // Dairy
        var dairy = Add(new Category { Name = "Dairy", Description = "Milk, butter, cheese, yoghurt" });
        dairy.Products.Add(new Product { Id = ++_nextProductId, Name = "Butter 250g", Price = 4.00m, StockQty = 100, Description = "Unsalted Butter", CategoryId = dairy.Id });
        dairy.Products.Add(new Product { Id = ++_nextProductId, Name = "Yoghurt 1kg", Price = 7.00m, StockQty = 100, Description = "Plain Greek Yogurt", CategoryId = dairy.Id });
        dairy.Products.Add(new Product { Id = ++_nextProductId, Name = "Milk 2L", Price = 3.20m, StockQty = 100, Description = "Full Cream Milk", CategoryId = dairy.Id });
        dairy.Products.Add(new Product { Id = ++_nextProductId, Name = "Sliced Cheese 250g", Price = 5.50m, StockQty = 100, Description = "Tasty Cheese 24 slices", CategoryId = dairy.Id });
        
        // Pantry
        var pantry = Add(new Category { Name = "Pantry", Description = "Cereal, pasta, spreads, sauces" });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "Corn Flakes 380g", Price = 7.00m, StockQty = 100, Description = "Crunchy Nut Corn Flakes Breakfast Cereal", CategoryId = pantry.Id });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "Instant Noodle 600g", Price = 8.00m, StockQty = 100, Description = "Spicy flavoured Instant Noodle 5 Packs", CategoryId = pantry.Id });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "Honey 500g", Price = 9.00m, StockQty = 100, Description = "Pure Honey 100% Australian Honey", CategoryId = pantry.Id });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "Peanut Butter 375g", Price = 6.50m, StockQty = 100, Description = "Smooth Natural Peanut Butter", CategoryId = pantry.Id });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "Pasta Fusilli 500g", Price = 2.50m, StockQty = 100, Description = "Durum Wheat Pasta", CategoryId = pantry.Id });
        pantry.Products.Add(new Product { Id = ++_nextProductId, Name = "BBQ Sauce 500ml", Price = 4.20m, StockQty = 100, Description = "Reduced Salt and Sugar BBQ Sauce", CategoryId = pantry.Id });

        // Cleaning
        var cleaning = Add(new Category { Name = "Cleaning", Description = "Cleaning supplies and household products" });
        cleaning.Products.Add(new Product { Id = ++_nextProductId, Name = "Toilet Paper 8 Packs", Price = 8.00m, StockQty = 100, Description = "100% Recycled Toilet Paper", CategoryId = cleaning.Id });
        cleaning.Products.Add(new Product { Id = ++_nextProductId, Name = "Laundry Powder 2kg", Price = 30.00m, StockQty = 100, Description = "For Front and Top Washing Machines", CategoryId = cleaning.Id });
        cleaning.Products.Add(new Product { Id = ++_nextProductId, Name = "Dishwashing Liquid 800ml", Price = 9.00m, StockQty = 100, Description = "Fresh Lemon Dishwashing Liquid", CategoryId = cleaning.Id });
        cleaning.Products.Add(new Product { Id = ++_nextProductId, Name = "Glass Cleaner Spray 500ml", Price = 5.00m, StockQty = 100, Description = "Ammonia Free", CategoryId = cleaning.Id });

        // Drinks
        var drinks = Add(new Category { Name = "Drinks", Description = "Beverages, soft drinks and juices" });
        drinks.Products.Add(new Product { Id = ++_nextProductId, Name = "Tropical Juice 2L", Price = 4.80m, StockQty = 100, Description = "Apple, Pear and Pineapple Juice", CategoryId = drinks.Id });
        drinks.Products.Add(new Product { Id = ++_nextProductId, Name = "Coke 1.25L", Price = 4.00m, StockQty = 100, Description = "Zero Sugar Soft Drink", CategoryId = drinks.Id });
        drinks.Products.Add(new Product { Id = ++_nextProductId, Name = "Sparkling Water 1.25L", Price = 3.50m, StockQty = 100, Description = "Lightly Sparkling Water", CategoryId = drinks.Id });
        drinks.Products.Add(new Product { Id = ++_nextProductId, Name = "Ginger Beer 10 Packs", Price = 17.50m, StockQty = 100, Description = "375ml x 10 Packs", CategoryId = drinks.Id });

        // Snacks
        var snacks = Add(new Category { Name = "Snacks", Description = "Chips, chocolate, biscuits" });
        snacks.Products.Add(new Product { Id = ++_nextProductId, Name = "Chocolate Block 180g", Price = 7.50m, StockQty = 100, Description = "60% Dark Chocolate", CategoryId = snacks.Id });
        snacks.Products.Add(new Product { Id = ++_nextProductId, Name = "Potato Chips 165g", Price = 6.00m, StockQty = 100, Description = "Ridge Cut Potato Chips", CategoryId = snacks.Id });
        snacks.Products.Add(new Product { Id = ++_nextProductId, Name = "Milk Arrowroot Biscuits 250g", Price = 2.50m, StockQty = 100, Description = "Plain Milk Arrowroot Biscuits", CategoryId = snacks.Id });
        snacks.Products.Add(new Product { Id = ++_nextProductId, Name = "Cream Wafers 125g", Price = 1.25m, StockQty = 100, Description = "Strawberry Cream Wafers", CategoryId = snacks.Id });
    }

    public IEnumerable<Category> GetAll() => _store.Values.OrderBy(c => c.Name);

    public Category? GetById(int id) => _store.TryGetValue(id, out var c) ? c : null;

    public Category Add(Category c)
    {
        c.Id = Interlocked.Increment(ref _nextId);
        _store[c.Id] = c;
        return c;
    }

    public void Update(Category c)
    {
        if (!_store.ContainsKey(c.Id)) throw new ArgumentException("Category not found");
        _store[c.Id] = c;
    }

    public bool Delete(int id) => _store.TryRemove(id, out _);
}