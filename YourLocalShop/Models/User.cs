using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public abstract class User
{
    public int Id { get; set; }

    [Required, StringLength(100)] public string FirstName { get; set; } = "";
    [Required, StringLength(100)] public string LastName { get; set; } = "";

    [Required, EmailAddress]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must be valid")]
    public string Email { get; set; } = "";

    [Required] public string PasswordHash { get; set; } = "";

    public abstract string Role { get; }
}