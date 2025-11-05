using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(0, 99999)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQty { get; set; }

    // Explicit relationship to Category
    [Required] public int CategoryId { get; set; }

    // Navigation property
    public Category? Category { get; set; }
}