﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Security.Principal;
using StockSharer.Web.App_Start;
using StockSharer.Web.Authentication;
using StockSharer.Web.Data;
using StockSharer.Web.Models;

namespace StockSharer.Web
{
    public class MvcApplication : HttpApplication
    {
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private readonly UserRepository _userRepository = new UserRepository();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null) return;
            var identity = new GenericIdentity(authTicket.Name, "Forms");
            var principal = new UserPrincipal(identity);
            var userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
            var user = (User) _serializer.Deserialize(userData, typeof (User));
            principal.User = RetrieveUser(user.UserId);
            Context.User = principal;
        }

        private User RetrieveUser(int userId)
        {
            //TODO: retrieving user from database to get updated balance, improve this by caching the user
            return _userRepository.RetrieveUser(userId);
        }
    }
}