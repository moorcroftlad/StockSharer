using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class UserAvailabilityController : BaseSettingsController
    {
        private readonly UserAvailabilityRepository _userAvailabilityRepository = new UserAvailabilityRepository();

        public ActionResult Index()
        {
            var userAvailabilityViewModel = TempData["ViewModel"] as UserAvailabilityViewModel ?? new UserAvailabilityViewModel
                {
                    UserAvailabilitys = _userAvailabilityRepository.RetrieveUserAvailabilitys(User.UserId)
                };
            return View(userAvailabilityViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(List<UserAvailability> userAvailabilitys)
        {
            if (ValidUserAvailabilitys(userAvailabilitys))
            {
                _userAvailabilityRepository.UpdateUserAvailabilitys(userAvailabilitys, User.UserId);
                TempData["ViewModel"] = new UserAvailabilityViewModel { Message = "Availability updated", Success = true, UserAvailabilitys = userAvailabilitys};
            }
            else
            {
                TempData["ViewModel"] = new UserAvailabilityViewModel { Message = "All end times must be greater than start times", Success = false, UserAvailabilitys = userAvailabilitys };
            }
            return RedirectToAction("Index");
        }

        private static bool ValidUserAvailabilitys(IEnumerable<UserAvailability> userAvailabilitys)
        {
            return userAvailabilitys.All(userAvailability => userAvailability.EndTime >= userAvailability.StartTime);
        }
    }

    public class UserAvailabilityViewModel
    {
        public List<UserAvailability> UserAvailabilitys { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}