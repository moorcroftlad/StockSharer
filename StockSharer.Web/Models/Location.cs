namespace StockSharer.Web.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string UrlToken { get; set; }
        public int? ParentLocationId { get; set; }
    }
}