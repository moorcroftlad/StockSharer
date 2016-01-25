using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using StockSharer.GameScraper.Models;

namespace StockSharer.GameScraper.Scrapers
{
    class MetaCriticScraper : IScrapeWebsites
    {
        private static readonly List<Platform> Platforms = new List<Platform>
            {
                new Platform{PlatformId = 1, ScraperToken = "xboxone"},
                new Platform{PlatformId = 2, ScraperToken = "xbox360"},
                new Platform{PlatformId = 3, ScraperToken = "ps3"},
                new Platform{PlatformId = 4, ScraperToken = "ps4"}
            };

        private readonly HtmlWeb _htmlWeb = new HtmlWeb();

        public List<Game> RetrieveAllGames()
        {
            var games = new List<Game>();
            foreach (var platform in Platforms)
            {
                games.AddRange(RetrieveGamesForPlatform(platform));
            }
            return games;
        }

        private IEnumerable<Game> RetrieveGamesForPlatform(Platform platform)
        {
            var completed = false;
            var page = 0;
            var games = new List<Game>();
            do
            {
                var url = string.Format("http://www.metacritic.com/browse/games/release-date/available/{0}/name?page={1}", platform.ScraperToken, page);
                var htmlDocument = _htmlWeb.Load(url);
                if (htmlDocument.DocumentNode != null)
                {
                    var nodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'product_title')]/a");
                    if (nodes != null)
                    {
                        games.AddRange(nodes.Select(node => new Game
                            {
                                Name = node.InnerHtml.Trim(), 
                                PlatformId = platform.PlatformId
                            }));
                        page++;
                    }
                    else
                    {
                        completed = true;
                    }
                }
                else
                {
                    completed = true;
                }
            } while (!completed);
            return games;
        }
    }
}