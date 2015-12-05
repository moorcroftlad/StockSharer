using System.Web.Mvc;
using System.Web.Security;

namespace StockSharer.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login(string returnUrl = "")
        {
            var loginViewModel = (TempData["LoginViewModel"] as LoginViewModel) ?? new LoginViewModel();
            return User.Identity.IsAuthenticated ? LogOut() : View(loginViewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (Membership.ValidateUser(loginViewModel.Email, loginViewModel.Password))
            {
                return RedirectToAction("Index", "Home");
            }
            loginViewModel.LoginError = true;
            TempData["LoginViewModel"] = loginViewModel;
            return Login();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool LoginError { get; set; }
    }
}