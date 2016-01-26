using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using StockSharer.Web.Authentication;
using StockSharer.Web.Data;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

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
            var userId = _userRepository.CreateUser(registerViewModel.Email, registerViewModel.Forename, registerViewModel.Surname, PasswordHash.CreateHash(registerViewModel.Password));
            if (userId > 0 )
            {
                FormsAuthentication.SetAuthCookie(registerViewModel.Email, false);
                return RedirectToAction("Index", "Home");
            }
            registerViewModel.Error = "An account with your email address already exists";
            return View(registerViewModel);
        }

        private bool ValidateUser(string email, string password)
        {
            var passwordHash = _userRepository.RetrievePasswordHash(email);
            return passwordHash != null && PasswordHash.ValidatePassword(password, passwordHash);
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