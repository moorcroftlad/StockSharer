using System.Web.Mvc;

namespace StockSharer.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            var loginViewModel = (TempData["LoginViewModel"] as LoginViewModel) ?? new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            loginViewModel.LoginError = true;
            TempData["LoginViewModel"] = loginViewModel;
            return Login();
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; }
        public bool LoginError { get; set; }
    }
}