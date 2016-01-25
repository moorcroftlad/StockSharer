using StockSharer.GameScraper.Scrapers;

namespace StockSharer.GameScraper
{
    class Program
    {
        //private static readonly IScrapeWebsites WebsiteScraper = new MetaCriticScraper();
        private static readonly IScrapeWebsites WebsiteScraper = new TheGamesDbScraper();
        private static readonly GameRepository GameRepository = new GameRepository();

        static void Main()
        {
            var games = WebsiteScraper.RetrieveAllGames();
            GameRepository.Insert(games);
        }
    }
}