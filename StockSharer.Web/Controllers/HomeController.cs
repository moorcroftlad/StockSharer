using System.Web.Mvc;

namespace StockSharer.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tools()
        {
            return View();
        }
    }
}