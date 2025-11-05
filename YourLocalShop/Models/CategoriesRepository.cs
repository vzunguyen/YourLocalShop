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
    private int _nextId = 1;

    public CategoriesRepository()
    {
        // Seed a few default categories
        Add(new Category { Name = "Dairy", Description = "Milk, eggs, cheese, yogurt" });
        Add(new Category { Name = "Fruit & Veg", Description = "Fresh fruits and vegetables" });
        Add(new Category { Name = "Bakery", Description = "Bread, pastries, and baked goods" });
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