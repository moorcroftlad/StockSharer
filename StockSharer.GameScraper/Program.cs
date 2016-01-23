using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using HtmlAgilityPack;

namespace StockSharer.GameScraper
{
    class Program
    {
        private static readonly List<Platform> Platforms = new List<Platform>
            {
                new Platform{PlatformId = 2, ScraperToken = "xboxone"},
                new Platform{PlatformId = 3, ScraperToken = "xbox360"},
                new Platform{PlatformId = 4, ScraperToken = "ps3"},
                new Platform{PlatformId = 5, ScraperToken = "ps4"}
            };

        static void Main()
        {
            var games = RetrieveAllGames();
            Insert(games);
        }

        private static void Insert(List<Game> games)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                foreach (var game in games)
                {
                    const string sql = @"   INSERT IGNORE INTO Game(PlatformId, Name)
                                            VALUES (@PlatformId, @Name);";
                    connection.Execute(sql, game);
                }
            }
        }

        private static List<Game> RetrieveAllGames()
        {
            var games = new List<Game>();
            foreach (var platform in Platforms)
            {
                games.AddRange(RetrieveGamesForPlatform(platform));
            }
            return games;
        }

        private static IEnumerable<Game> RetrieveGamesForPlatform(Platform platform)
        {
            var completed = false;
            var page = 0;
            var games = new List<Game>();
            do
            {
                var url = string.Format("http://www.metacritic.com/browse/games/release-date/available/{0}/name?page={1}", platform.ScraperToken, page);
                var htmlWeb = new HtmlWeb();
                var htmlDocument = htmlWeb.Load(url);
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

    internal class Platform
    {
        public int PlatformId { get; set; }
        public string ScraperToken { get; set; }
    }

    internal class Game
    {
        public string Name { get; set; }
        public int PlatformId { get; set; }
    }
}
