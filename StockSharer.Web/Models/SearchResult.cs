using System;

namespace StockSharer.Web.Models
{
    public class SearchResult
    {
        public string GameName { get; set; }
        public string ImageUrl { get; set; }
        public string StockTypeName { get; set; }
        public string PlatformName { get; set; }
        public decimal Price { get; set; }
        public string TownName { get; set; }
        public string StockStatusName { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid StockReference { get; set; }
    }
}