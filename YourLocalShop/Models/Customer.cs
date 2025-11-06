using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Customer : User
{
    [Required, RegularExpression(@"^04\d{8}$", ErrorMessage = "Phone must start with 04 and be 10 digits total.")]
    public string Phone { get; set; } = "";

    [Required] public Address Address { get; set; } = new();

    public override string Role => "Customer";
}