namespace StockSharer.ViewModels
{
    public class RegisterViewModel
    {
        public bool LoginError { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}