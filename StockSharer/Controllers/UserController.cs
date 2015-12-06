using System.Web.Mvc;
using System.Web.Security;
using StockSharer.Authentication;
using StockSharer.Data;
using StockSharer.ViewModels;

namespace StockSharer.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public ActionResult Login(string returnUrl = "")
        {
            var loginViewModel = (TempData["LoginViewModel"] as LoginViewModel) ?? new LoginViewModel();
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
            TempData["LoginViewModel"] = loginViewModel;
            return Login();
        }

        public ActionResult Register()
        {
            var registerViewModel = (TempData["RegisterViewModel"] as RegisterViewModel) ?? new RegisterViewModel();
            return View(registerViewModel);
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