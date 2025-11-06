namespace YourLocalShop.Models;

public class ShoppingCart
{
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}
