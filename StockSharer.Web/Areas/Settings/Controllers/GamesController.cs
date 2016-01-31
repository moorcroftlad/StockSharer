using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.ViewModels;
using StockSharer.Web.Data;

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
    }
}