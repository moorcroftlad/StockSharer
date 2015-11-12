using System.Web.Mvc;

namespace StockSharer.Controllers
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