using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using StockSharer.Web.Data;

namespace StockSharer.Web.Controllers
{
    public class AuthenticationHelper
    {
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private readonly UserRepository _userRepository = new UserRepository();

        public void SetFormsAuthenticationCookie(HttpResponseBase response, string email)
        {
            var user = _userRepository.RetrieveUser(email);
            FormsAuthentication.SetAuthCookie(user.Email, false);
            var userData = _serializer.Serialize(user);
            var ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddDays(30), true, userData, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
        }
    }
}