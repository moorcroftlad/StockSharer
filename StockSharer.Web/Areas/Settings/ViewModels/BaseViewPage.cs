using System.Web.Mvc;
using StockSharer.Web.Authentication;
using StockSharer.Web.Models;

namespace StockSharer.Web.Areas.Settings.ViewModels
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new User User
        {
            get
            {
                var principal = base.User as UserPrincipal;
                return principal != null ? principal.User : null;
            }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new User User
        {
            get
            {
                var principal = base.User as UserPrincipal;
                return principal != null ? principal.User : null;
            }
        }
    }
}