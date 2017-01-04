namespace StockSharer.Web.Models
{
    public class SearchFilter
    {
        public string Town { get; set; }
        public int? Platform { get; set; }
        public int? SearchType { get; set; }
        public int? Status { get; set; }
        public int? LenderType { get; set; }
        public string Sort { get; set; }
    }
}