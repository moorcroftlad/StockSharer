using System;

namespace StockSharer.Web.Areas.Settings.Models
{
    public class GameRequest
    {
        public Guid Reference { get; set; }
        public string GameName { get; set; }
        public int Nights { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
    }
}