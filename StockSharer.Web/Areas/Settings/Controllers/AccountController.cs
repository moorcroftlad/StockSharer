using System.Web.Mvc;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class GamesController : BaseSettingsController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}