using System;

namespace StockSharer.Web.Models
{
    public class SearchResult
    {
        public Guid Reference { get; set; }
        public string GameName { get; set; }
        public string PlatformName { get; set; }
        public string Availability { get; set; }
        public string ImageUrl { get; set; }
        public string HostedImageUrl { get; set; }
        public int Distance { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public bool RequestedToday { get; set; }
        public bool OwnsGame { get; set; }
    }
}