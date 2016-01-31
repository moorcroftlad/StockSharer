using System;

namespace StockSharer.Web.Models
{
    public class GameAvailability
    {
        public string HostedImageUrl { get; set; }
        public string GameName { get; set; }
        public string PlatformName { get; set; }
        public DateTime DateAdded { get; set; }
        public string AvailabilityName { get; set; }
    }
}