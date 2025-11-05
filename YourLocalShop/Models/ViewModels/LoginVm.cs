using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models.ViewModels;

public class LoginVm
{
    [Required, RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    public string Email { get; set; } = "";

    [Required] public string Password { get; set; } = "";
}