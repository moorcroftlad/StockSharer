using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Security.Principal;
using StockSharer.Web.App_Start;
using StockSharer.Web.Authentication;
using StockSharer.Web.Models;

namespace StockSharer.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            //if (Request.Cookies[FormsAuthentication.FormsCookieName] == null) return;
            //var encryptedTicket = Request.Cookies[FormsAuthentication.FormsCookieName].Value;
            //var formsAuthenticationTicket = FormsAuthentication.Decrypt(encryptedTicket);
            //if (formsAuthenticationTicket == null) return;
            //var username = formsAuthenticationTicket.Name;
            //HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username, "Forms"), null);


            
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null) return;
            var identity = new GenericIdentity(authTicket.Name, "Forms");
            var principal = new UserPrincipal(identity);
            var userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
            principal.User = (User)_serializer.Deserialize(userData, typeof(User));
            Context.User = principal;
        }
    }
}