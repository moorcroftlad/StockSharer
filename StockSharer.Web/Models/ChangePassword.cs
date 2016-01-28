namespace StockSharer.Web.Models
{
    public class ChangePassword
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}