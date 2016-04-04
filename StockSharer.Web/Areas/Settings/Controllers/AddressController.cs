using System.Web.Mvc;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class AddressController : BaseSettingsController
    {
        private readonly AddressRepository _addressRepository = new AddressRepository();

        public ActionResult Index()
        {
            var addressViewModel = TempData["ViewModel"] as AddressViewModel ?? new AddressViewModel();
            addressViewModel.Address = _addressRepository.RetrieveAddress(User.UserId) ?? new Address();
            return View(addressViewModel);
        }

        [HttpPost]
        public ActionResult Index(Address address)
        {
            //TODO: retrieve lat / long of postcode and return error message if cannot be found
            address.Latitude = 0;
            address.Longitude = 0;
            address.UserId = User.UserId;
            _addressRepository.UpdateAddress(address);
            TempData["ViewModel"] = new AddressViewModel {Message = "Address updated", Success = true};
            return RedirectToAction("Index");
        }
    }

    public class AddressViewModel
    {
        public Address Address { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}