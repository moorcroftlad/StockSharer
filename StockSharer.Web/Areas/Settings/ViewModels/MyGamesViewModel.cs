using System.Collections.Generic;
using StockSharer.Web.Models;

namespace StockSharer.Web.Areas.Settings.ViewModels
{
    public class MyGamesViewModel
    {
        public List<GameAvailability> MyGames { get; set; }
    }
}