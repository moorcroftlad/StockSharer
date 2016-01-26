using System.Web.Mvc;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class AccountController : BaseSettingsController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}