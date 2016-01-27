namespace StockSharer.Web.Email
{
    public interface ISendEmail
    {
        void SendEmail(string to, string subject, string bodyText, string bodyHtml);
    }
}