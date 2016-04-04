using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.ViewModels;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class BalanceController : BaseSettingsController
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public ActionResult Index()
        {
            var settingsViewModel = TempData["ViewModel"] as SettingsViewModel ?? new SettingsViewModel();
            return View(settingsViewModel);
        }

        [HttpPost]
        public ActionResult Index(int amount)
        {
            amount = 5 <= amount && amount <= 50 ? amount : 5;
            _userRepository.UpdateBalance(amount, User.UserId);
            var settingsViewModel = new SettingsViewModel
                {
                    Message = "Balance updated",
                    Success = true
                };
            TempData["ViewModel"] = settingsViewModel;
            return RedirectToAction("Index");
        }
    }
}