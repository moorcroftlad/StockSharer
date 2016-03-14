using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.ViewModels;
using StockSharer.Web.Authentication;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class AccountController : BaseSettingsController
    {
        private readonly UserRepository _userRepository = new UserRepository();
        private readonly AuthenticationHelper _authenticationHelper = new AuthenticationHelper();

        public ActionResult Index()
        {
            var settingsViewModel = new SettingsViewModel();
            return View(settingsViewModel);
        }

        [HttpPost]
        public ActionResult Index(string forename, string surname)
        {
            User.Forename = forename;
            User.Surname = surname;
            _userRepository.UpdateUser(User);
            _authenticationHelper.SetFormsAuthenticationCookie(Response, User.Email);
            var settingsViewModel = new SettingsViewModel
                {
                    Message = "Details updated",
                    Success = true
                };
            return View(settingsViewModel);
        }
    }
}