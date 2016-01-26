using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.Attributes;
using StockSharer.Web.Authentication;
using StockSharer.Web.Models;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    [AuthorizeUser]
    public class BaseSettingsController : Controller
    {
        protected virtual new User User
        {
            get
            {
                var principal = HttpContext.User as UserPrincipal;
                return principal != null ? principal.User : null;
            }
        }
    }
}