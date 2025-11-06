using System.ComponentModel.DataAnnotations;

namespace YourLocalShop.Models;

public class Payment
{
    [Required]
    [CreditCard(ErrorMessage = "Invalid card number.")]
    public string CardNumber { get; set; } = "";

    [Required]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Expiry must be in MM/YY format.")]
    public string Expiry { get; set; } = "";

    [Required]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVC must be 3 or 4 digits.")]
    public string CVC { get; set; } = "";

    public int OrderId { get; set; }

    // Luhn algorithm check
    public bool IsValidCardNumber()
    {
        if (string.IsNullOrWhiteSpace(CardNumber)) return false;

        var digits = CardNumber.Where(char.IsDigit).Select(c => c - '0').ToArray();
        int sum = 0;
        bool alternate = false;

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int n = digits[i];
            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }
            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }    
}