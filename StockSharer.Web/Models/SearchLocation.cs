namespace StockSharer.Web.Models
{
    public class SearchLocation
    {
        public int SearchLocationId { get; set; }
        public string Name { get; set; }
        public string UrlToken { get; set; }
        public int? ParentLocationId { get; set; }
    }
}