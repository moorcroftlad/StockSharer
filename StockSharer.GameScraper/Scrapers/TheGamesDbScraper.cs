using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using StockSharer.GameScraper.Models;

namespace StockSharer.GameScraper.Scrapers
{
    class TheGamesDbScraper : IScrapeWebsites
    {
        private const string BaseUrl = "http://thegamesdb.net";

        private static readonly List<Platform> Platforms = new List<Platform>
            {
                new Platform{PlatformId = 1, ScraperToken = "4920"},    //xbox one
                new Platform{PlatformId = 2, ScraperToken = "15"},      //xbox 360
                new Platform{PlatformId = 3, ScraperToken = "12"},      //ps3
                new Platform{PlatformId = 4, ScraperToken = "4919"}     //ps4
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
            var page = 1;
            var games = new List<Game>();
            do
            {
                var url = string.Format("{0}/browse/{1}/?sortBy=g.GameTitle&limit=500&searchview=listing&page={2}", BaseUrl, platform.ScraperToken, page);
                var htmlDocument = _htmlWeb.Load(url);
                if (htmlDocument.DocumentNode != null)
                {
                    var nodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'backgroundGradientGrey')]/h3/a");
                    if (nodes != null)
                    {
                        games.AddRange(nodes.Select(node => new Game
                            {
                                Name = node.InnerHtml.Trim(), 
                                PlatformId = platform.PlatformId,
                                ImageUrl = RetrieveImageUrl(node.Attributes["href"])
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

        private string RetrieveImageUrl(HtmlAttribute htmlAttribute)
        {
            try
            {
                var url = htmlAttribute.Value;
                var htmlDocument = _htmlWeb.Load(url);
                if (htmlDocument.DocumentNode != null)
                {
                    var node = htmlDocument.DocumentNode.SelectSingleNode("//*[@id = 'frontCover']");
                    if (node != null)
                    {
                        return string.Format("{0}{1}", BaseUrl, node.Attributes["src"].Value);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}