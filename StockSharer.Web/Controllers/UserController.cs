using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Amazon.SimpleEmail;
using Amazon;
using Amazon.SimpleEmail.Model;
using StockSharer.Web.Authentication;
using StockSharer.Web.Cache;
using StockSharer.Web.Data;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private readonly RedisCache _redisCache = new RedisCache();

        public ActionResult Login(string returnUrl = "")
        {
            var loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };
            return User.Identity.IsAuthenticated ? LogOut() : View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ValidateUser(loginViewModel.Email, loginViewModel.Password))
            {
                SetFormsAuthenticationCookie(loginViewModel.Email);
                if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) 
                    && Url.IsLocalUrl(loginViewModel.ReturnUrl) 
                    && loginViewModel.ReturnUrl.StartsWith("/")
                    && !loginViewModel.ReturnUrl.StartsWith("//") 
                    && !loginViewModel.ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            loginViewModel.LoginError = true;
            return View(loginViewModel);
        }

        private void SetFormsAuthenticationCookie(string email)
        {
            var user = _userRepository.RetrieveUser(email);
            FormsAuthentication.SetAuthCookie(user.Email, false);
            var userData = _serializer.Serialize(user);
            var ticket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddDays(30), true, userData, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                registerViewModel.Error = "Passwords must match";
                return View(registerViewModel);
            }
            var email = registerViewModel.Email.ToLower();
            var userId = _userRepository.CreateUser(email, registerViewModel.Forename, registerViewModel.Surname, PasswordHash.CreateHash(registerViewModel.Password));
            if (userId > 0)
            {
                var temporaryAuthGuid = CreateTemporaryAuthGuid(userId);
                SendAuthEmail(temporaryAuthGuid, email);
                return RedirectToAction("RegistrationSuccessful", "User");
            }
            registerViewModel.Error = "An account with your email address already exists";
            return View(registerViewModel);
        }

        private static void SendAuthEmail(Guid temporaryAuthGuid, string email)
        {
            //using (var smtpClient = new SmtpClient())
            //{
            //    const string subject = "Welcome to StockSharer";
            //    var body = string.Format("Thank you for registering an account with StockSharer.  To complete your registration please click on the link below:<br /><br /><a href=\"http://www.stocksharer.com/user/authenticate/{0}\">http://www.stocksharer.com/user/authenticate/{0}</a>", temporaryAuthGuid);
            //    using (var mailMessage = new MailMessage("noreply@stocksharer.com", email, subject, body))
            //    {
            //        smtpClient.Send(mailMessage);
            //    }
            //}

            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.EUWest1))
            {
                var content = new Content("Welcome to StockSharer");
                var bodyText = string.Format("Thank you for registering an account with StockSharer.  To complete your registration please click on the link below:<br /><br /><a href=\"http://www.stocksharer.com/user/authenticate/{0}\">http://www.stocksharer.com/user/authenticate/{0}</a>", temporaryAuthGuid);
                var body = new Body(new Content(bodyText));
                var message = new Message(content, body);
                var destination = new Destination(new List<string> {email});
                var sendEmailRequest = new SendEmailRequest("noreply@stocksharer.com", destination, message);
                client.SendEmailAsync(sendEmailRequest);
            }
        }

        private Guid CreateTemporaryAuthGuid(int userId)
        {
            var authGuid = Guid.NewGuid();
            _redisCache.Set(authGuid.ToString(), userId.ToString(CultureInfo.InvariantCulture), new TimeSpan(1, 0, 0, 0));
            return authGuid;
        }

        private bool ValidateUser(string email, string password)
        {
            var passwordHash = _userRepository.RetrievePasswordHash(email);
            return passwordHash != null && PasswordHash.ValidatePassword(password, passwordHash);
        }

        public ActionResult RegistrationSuccessful()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}