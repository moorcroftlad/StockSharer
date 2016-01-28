using System.Web.Mvc;
using StockSharer.Web.Controllers;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class AccountController : BaseSettingsController
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly AuthenticationHelper _authenticationHelper = new AuthenticationHelper();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string forename, string surname)
        {
            User.Forename = forename;
            User.Surname = surname;
            _userRepository.UpdateUser(User);
            _authenticationHelper.SetFormsAuthenticationCookie(Response, User.Email);
            return View();
        }
    }
}