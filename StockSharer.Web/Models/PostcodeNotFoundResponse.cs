namespace StockSharer.Web.Models
{
    public class PostcodeNotFoundResponse : SavePostcodeResponse
    {
        public PostcodeNotFoundResponse()
        {
            Reason = "Postcode not found, please choose a nearby postcode";
        }
    }
}