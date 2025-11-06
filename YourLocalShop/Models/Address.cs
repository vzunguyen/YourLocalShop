using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Address
{
    [RegularExpression(@"^\d{1,4}$", ErrorMessage = "Unit number must be 1–4 digits.")]
    public string? UnitNumber { get; set; }   // optional
    
    [RegularExpression(@"^\d+[A-Za-z]?$")]
    public string StreetNumber { get; set; } = "";

    [RegularExpression(@"^[A-Za-z0-9\s.'-]{2,}$")]
    public string StreetName { get; set; } = "";

    [RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string City { get; set; } = "";

    [RegularExpression(@"^(VIC|NSW|QLD|SA|WA|TAS|ACT|NT)$")]
    public string State { get; set; } = "VIC";

    [RegularExpression(@"^[A-Za-z\s.'-]{2,}$")]
    public string Country { get; set; } = "Australia";

    [RegularExpression(@"^\d{4}$", ErrorMessage = "Post code must be exactly 4 digits.")]
    public string PostCode { get; set; } = "";
}