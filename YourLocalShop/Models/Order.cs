using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }   // from logged-in account
    public Customer Customer { get; set; } = new(); // holds name + address
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Subtotal);
    public string Status { get; set; } = "";    // Status starts empty
    public string InvoiceId { get; set; } = "";
    public string ReceiptId { get; set; } = "";
}
