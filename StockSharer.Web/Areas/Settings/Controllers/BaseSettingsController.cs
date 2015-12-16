using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.Attributes;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    [AuthorizeUser]
    public class BaseSettingsController : Controller
    {
        
    }
}