using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Category
{
    public int Id { get; set; }     // Unique ID

    [Required, StringLength(100)]
    public string Name { get; set; }    //eg. "Dairy", "Snack"

    [StringLength(500)]
    public string? Description { get; set; }

    // Navigation property: each category holds its products
    public List<Product> Products{ get; set; } = new();
}