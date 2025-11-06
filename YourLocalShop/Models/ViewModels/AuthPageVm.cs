namespace YourLocalShop.Models.ViewModels
{
    public class AuthPageVm
    {
        public LoginVm Login { get; set; } = new();
        public RegisterVm Register { get; set; } = new();
        public string ActiveTab { get; set; } = "login"; // login or register
    }
}