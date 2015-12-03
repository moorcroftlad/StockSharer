using System.Web.Mvc;

namespace StockSharer.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}