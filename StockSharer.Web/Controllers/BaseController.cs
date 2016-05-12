using System.Web.Mvc;
using StockSharer.Web.Authentication;
using StockSharer.Web.Models;

namespace StockSharer.Web.Controllers
{
    public class BaseController : Controller
    {
        protected new virtual User User
        {
            get
            {
                var principal = HttpContext.User as UserPrincipal;
                return principal != null ? principal.User : null;
            }
        }
    }
}