namespace StockSharer.Web.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string HostedImageUrl { get; set; }
        public string GameName { get; set; }
        public string PlatformName { get; set; }
    }

    public class AddGameResult : Game
    {
        public bool OwnsGame { get; set; }
    }
}