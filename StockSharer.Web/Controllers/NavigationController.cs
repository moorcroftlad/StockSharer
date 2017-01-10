using System.Web.Mvc;

namespace StockSharer.Web.Controllers
{
    public class NavigationController : Controller
    {
        public ActionResult Top()
        {
            return View();
        }
    }
}