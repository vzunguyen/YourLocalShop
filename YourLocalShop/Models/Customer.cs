using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Customer : User
{
    [RegularExpression(@"^04\d{8}$",  ErrorMessage = "Phone number must start with 04 and be 10 digits long.")]
    public string Phone { get; set; } = "";
    
    public Address Address { get; set; } = new();

    public override string Role => "Customer";
}