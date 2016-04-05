using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Location;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class AddressController : BaseSettingsController
    {
        private readonly AddressRepository _addressRepository = new AddressRepository();
        private readonly ICalulateLocation _locationCalculator = new GoogleMapsApiLocationCalculator();

        public ActionResult Index()
        {
            var addressViewModel = TempData["ViewModel"] as AddressViewModel ?? new AddressViewModel {Address = _addressRepository.RetrieveAddress(User.UserId) ?? new Address()};
            return View(addressViewModel);
        }

        [HttpPost]
        public ActionResult Index(Address address)
        {
            var geoLocation = _locationCalculator.CalculateLocation(address.Postcode);
            if (ValidAddress(address) && geoLocation != null)
            {
                address.Postcode = address.Postcode.Replace(" ", "").ToUpper();
                address.Latitude = geoLocation.Latitude;
                address.Longitude = geoLocation.Longitude;
                address.UserId = User.UserId;
                _addressRepository.UpdateAddress(address);
                TempData["ViewModel"] = new AddressViewModel { Message = "Address updated", Success = true, Address = address};
            }
            else
            {
                TempData["ViewModel"] = new AddressViewModel {Message = "Please enter valid address", Success = false, Address = address};
            }
            return RedirectToAction("Index");
        }

        private static bool ValidAddress(Address address)
        {
            return address != null &&
                   !string.IsNullOrEmpty(address.Line1) &&
                   !string.IsNullOrEmpty(address.Line2) &&
                   !string.IsNullOrEmpty(address.Town) &&
                   !string.IsNullOrEmpty(address.County) &&
                   !string.IsNullOrEmpty(address.Postcode);
        }
    }

    public class AddressViewModel
    {
        public Address Address { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}