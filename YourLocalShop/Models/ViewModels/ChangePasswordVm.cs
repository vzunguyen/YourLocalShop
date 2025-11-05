using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models.ViewModels
{
    public class ChangePasswordVm
    {
        [Required] public string OldPassword { get; set; } = "";

        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
             ErrorMessage = "New password needs upper, lower, number, symbol and 8+ chars.")]
        public string NewPassword { get; set; } = "";

        [Required, Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}