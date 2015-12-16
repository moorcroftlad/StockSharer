using System.Web.Mvc;
using System.Web.Security;
using StockSharer.Web.Authentication;
using StockSharer.Web.Data;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public ActionResult Login(string returnUrl = "")
        {
            var loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };
            return User.Identity.IsAuthenticated ? LogOut() : View(loginViewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ValidateUser(loginViewModel.Email, loginViewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(loginViewModel.Email, false);
                if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl) && loginViewModel.ReturnUrl.StartsWith("/")
                    && !loginViewModel.ReturnUrl.StartsWith("//") && !loginViewModel.ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            loginViewModel.LoginError = true;
            return View(loginViewModel);
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                registerViewModel.Error = "Passwords must match";
                return View(registerViewModel);
            }
            var userId = _userRepository.CreateUser(registerViewModel.Email, PasswordHash.CreateHash(registerViewModel.Password));
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