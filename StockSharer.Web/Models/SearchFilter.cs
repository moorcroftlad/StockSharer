namespace StockSharer.Web.Models
{
    public class SearchFilter
    {
        public int? LocationId { get; set; }
        public int? PlatformId { get; set; }
        public int? StockStatusId { get; set; }
        public int? StoreTypeId { get; set; }
    }
}