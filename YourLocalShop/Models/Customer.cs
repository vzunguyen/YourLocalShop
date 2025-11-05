using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Customer : User
{
    [Required, RegularExpression(@"^\d{8,15}$", ErrorMessage = "Phone must be 8–15 digits.")]
    public string Phone { get; set; } = "";

    [Required] public Address Address { get; set; } = new();

    public override string Role => "Customer";
}