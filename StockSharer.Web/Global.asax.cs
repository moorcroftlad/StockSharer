using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using StockSharer.Web.App_Start;

namespace StockSharer.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] == null) return;
            var encryptedTicket = Request.Cookies[FormsAuthentication.FormsCookieName].Value;
            var formsAuthenticationTicket = FormsAuthentication.Decrypt(encryptedTicket);
            if (formsAuthenticationTicket == null) return;
            var username = formsAuthenticationTicket.Name;
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username, "Forms"), null);
        }
    }
}