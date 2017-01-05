namespace StockSharer.Web.Models
{
    public class SearchFilter
    {
        public int? TownId { get; set; }
        public int? PlatformId { get; set; }
        public int? StockTypeId { get; set; }
        public int? StoreTypeId { get; set; }
    }
}