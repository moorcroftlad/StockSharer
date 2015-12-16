using System.Web;
using System.Web.Mvc;

namespace StockSharer.Web.Areas.Settings.Attributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null)
            {
                filterContext.Result = new RedirectResult("/user/login?ReturnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
            }
        }
    }
}