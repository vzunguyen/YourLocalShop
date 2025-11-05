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