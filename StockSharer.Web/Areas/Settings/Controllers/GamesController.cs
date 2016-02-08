using System.Collections.Generic;
using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.ViewModels;
using StockSharer.Web.Data;
using StockSharer.Web.Models;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class GamesController : BaseSettingsController
    {
        private readonly AvailabilityRepository _availabilityRepository = new AvailabilityRepository();
        private readonly GameRepository _gameRepository = new GameRepository();

        public ActionResult Index()
        {
            var myGamesViewModel = new MyGamesViewModel
                {
                    MyGames = _availabilityRepository.RetrieveMyGames(User.UserId),
                    Games = _gameRepository.SearchForGames(null)
                };
            return View(myGamesViewModel);
        }

        public ActionResult Search(string term)
        {
            var searchViewModel = new SearchViewModel
                {
                    Term = term,
                    Games = _gameRepository.SearchForGames(term)
                };
            return View(searchViewModel);
        }

        [HttpPost]
        public JsonResult AddGameAvailability(int id)
        {
            var gameAvailability = _availabilityRepository.AddGameAvailability(id, User.UserId);
            return Json(gameAvailability);
        }
    }

    public class SearchViewModel
    {
        public string Term { get; set; }
        public List<Game> Games { get; set; }
    }
}