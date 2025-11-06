namespace YourLocalShop.Models.ViewModels;

public class CheckoutVm
{
    public Customer Customer { get; set; } = new(); // Recipient details (pre-filled from logged-in Customer)
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }

}
