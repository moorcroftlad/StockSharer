namespace StockSharer.GameScraper.Models
{
    internal class Game
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public int PlatformId { get; set; }
        public string ImageUrl { get; set; }
        public string HostedImageUrl { get; set; }
    }
}