namespace YourLocalShop.Models.ViewModels;

public class InvoiceVm
{
    public Order Order { get; set; } = new();
    public Payment Payment { get; set; } = new();
}