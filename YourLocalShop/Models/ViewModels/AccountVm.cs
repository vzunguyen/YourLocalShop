using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models.ViewModels;

public class AccountVm
{
    [EmailAddress] public string Email { get; set; } = "";

    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";

    [Required, RegularExpression(@"^04\d{8}$",  ErrorMessage = "Phone number must start with 04 and be 10 digits long.")]
    public string Phone { get; set; } = "";

    [Required, RegularExpression(@"^\d+[A-Za-z]?$", ErrorMessage = "Street number must be numeric, optionally ending with a letter.")]
    public string StreetNumber { get; set; } = "";

    [Required, RegularExpression(@"^[A-Za-z0-9\s.'-]{2,}$")]
    public string StreetName { get; set; } = "";

    [Required, RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string City { get; set; } = "";

    [Required, RegularExpression(@"^(VIC|NSW|QLD|SA|WA|TAS|ACT|NT)$", ErrorMessage = "State must be one of VIC, NSW, QLD, SA, WA, TAS, ACT, or NT.")]
    public string State { get; set; } = "VIC";

    [Required, RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string Country { get; set; } = "Australia";

    [Required, RegularExpression(@"^\d{4}$", ErrorMessage = "Post code must be exactly 4 digits.")]
    public string PostCode { get; set; } = "";
}