using System.Collections.Generic;
using StockSharer.GameScraper.Models;

namespace StockSharer.GameScraper.Scrapers
{
    internal interface IScrapeWebsites
    {
        List<Game> RetrieveAllGames();
    }
}