namespace StockSharer.Web.Models
{
    public class SearchLocation
    {
        public int SearchLocationId { get; set; }
        public string Name { get; set; }
        public string UrlToken { get; set; }
        public int? ParentLocationId { get; set; }
    }

    public class SearchLocationItem
    {
        public SearchLocation SearchLocation { get; set; }
        public int NumberOfResults { get; set; }
    }
}