using System.Text.Json;
using YourLocalShop.Models;

namespace YourLocalShop.Services;

public interface ICartService
{
    Cart Get();
    void Add(int productId, string name, decimal unitPrice, int quantity = 1);
    void UpdateQuantity(int productId, int quantity);
    void Remove(int productId);
    void Clear();
}

public class CartService : ICartService
{
    private const string SessionKey = "YLShop.Cart";
    private readonly IHttpContextAccessor _http;

    public CartService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Cart Get() => GetOrCreate();

    public void Add(int productId, string name, decimal unitPrice, int quantity = 1)
    {
        if (quantity <= 0) quantity = 1;

        var cart = GetOrCreate();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Name = name,
                UnitPrice = unitPrice,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity += quantity;
        }

        Save(cart);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var cart = GetOrCreate();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0)
            cart.Items.Remove(item);
        else
            item.Quantity = quantity;

        Save(cart);
    }

    public void Remove(int productId)
    {
        var cart = GetOrCreate();
        cart.Items.RemoveAll(i => i.ProductId == productId);
        Save(cart);
    }

    public void Clear()
    {
        Save(new Cart());
    }

    private Cart GetOrCreate()
    {
        var json = _http.HttpContext?.Session.GetString(SessionKey);
        if (string.IsNullOrEmpty(json)) return new Cart();
        try
        {
            return JsonSerializer.Deserialize<Cart>(json) ?? new Cart();
        }
        catch
        {
            return new Cart();
        }
    }

    private void Save(Cart cart)
    {
        var json = JsonSerializer.Serialize(cart);
        _http.HttpContext?.Session.SetString(SessionKey, json);
    }
}