﻿namespace StockSharer.Web.Models
{
    public class SearchResult
    {
        public string GameName { get; set; }
        public string Availability { get; set; }
        public string ImageUrl { get; set; }
        public string HostedImageUrl { get; set; }
        public int Distance { get; set; }
        public int UserId { get; set; }
    }
}