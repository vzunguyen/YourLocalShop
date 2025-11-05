namespace YourLocalShop.Models;

public class Cart
{
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
}

public class CartItem
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
}