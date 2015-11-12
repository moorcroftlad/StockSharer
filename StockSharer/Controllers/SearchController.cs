using System.Web.Mvc;

namespace StockSharer.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string q)
        {
            return View();
        }
	}
}