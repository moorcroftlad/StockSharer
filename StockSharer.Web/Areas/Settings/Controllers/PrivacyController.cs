using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.ViewModels;
using StockSharer.Web.Authentication;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class PrivacyController : BaseSettingsController
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public ActionResult Index()
        {
            var settingsViewModel = new SettingsViewModel();
            return View(settingsViewModel);
        }

        [HttpPost]
        public ActionResult Index(string password, string confirmPassword)
        {
            var settingsViewModel = new SettingsViewModel();
            if (password == confirmPassword)
            {
                var passwordHash = PasswordHash.CreateHash(password);
                _userRepository.UpdatePassword(User.UserId, passwordHash);
                settingsViewModel.Message = "Password updated";
                settingsViewModel.Success = true;
            }
            else
            {
                settingsViewModel.Message = "Passwords must match";
            }
            return View(settingsViewModel);
        }
    }
}