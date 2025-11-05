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
        Add(new Category { Name = "Pantry", Description = "Cereal, pasta, spreads, sauces" });
        Add(new Category { Name = "Cleaning", Description = "Cleaning supplies and household products" });
        Add(new Category { Name = "Drinks", Description = "Beverages, soft drinks and juices" });
        Add(new Category { Name = "Snacks", Description = "Snacks and treats" });
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