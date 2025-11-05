using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models.ViewModels;

public class AccountVm
{
    [EmailAddress] public string Email { get; set; } = "";

    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";

    [Required, RegularExpression(@"^\d{8,15}$")]
    public string Phone { get; set; } = "";

    [Required, RegularExpression(@"^\d+[A-Za-z]?$")]
    public string StreetNumber { get; set; } = "";

    [Required, RegularExpression(@"^[A-Za-z0-9\s.'-]{2,}$")]
    public string StreetName { get; set; } = "";

    [Required, RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string City { get; set; } = "";

    [Required, RegularExpression(@"^(VIC|NSW|QLD|SA|WA|TAS|ACT|NT)$")]
    public string State { get; set; } = "VIC";

    [Required, RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string Country { get; set; } = "Australia";

    [Required, RegularExpression(@"^\d{4}$")]
    public string PostCode { get; set; } = "";
}