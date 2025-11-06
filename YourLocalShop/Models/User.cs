using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public abstract class User
{
    private static int _counter = 1;
    public int Id { get; set; }

    [StringLength(100)] public string FirstName { get; set; } = "";
    [StringLength(100)] public string LastName { get; set; } = "";
    
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must be valid")]
    public string Email { get; set; } = "";
    
    public string PasswordHash { get; set; } = "";

    public abstract string Role { get; }

    protected User()
    {
        Id = _counter++;
    }
}