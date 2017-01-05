namespace StockSharer.Web.Models
{
    public class SearchFilter
    {
        public string Town { get; set; }
        public int? PlatformId { get; set; }
        public int? StockTypeId { get; set; }
        public int? StoreTypeId { get; set; }
        public string Sort { get; set; }
    }
}