using System;
using System.IO;
using System.Net;
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
            GameRepository.MergeGames(games);
            SaveImages();
        }

        private static void SaveImages()
        {
            var games = GameRepository.RetrieveImagesToSave();
            foreach (var game in games)
            {
                using (var client = new WebClient())
                {
                    var data = client.DownloadData(game.ImageUrl);
                    var extension = new FileInfo(new Uri(game.ImageUrl).AbsolutePath).Extension;
                    var filename = string.Format("{0}{1}", Guid.NewGuid(), extension);
                    File.WriteAllBytes(string.Format(@"c:\images\{0}", filename), data);
                    game.HostedImageUrl = string.Format("http://images.stocksharer.com/{0}", filename);
                }
            }
            GameRepository.UpdateImages(games);
        }
    }
}