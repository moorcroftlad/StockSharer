using System.Web.Mvc;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class PrivacyController : BaseSettingsController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}