using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.Models.ViewModels;
using StockSharer.Web.Data;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class RequestsController : BaseSettingsController
    {
        private readonly RequestRepository _requestRepository;

        public RequestsController()
        {
            _requestRepository = new RequestRepository();
        }

        public ActionResult Index()
        {
            var requestViewModel = new RequestsViewModel
                {
                    Requests = _requestRepository.RetrieveMyRequests(User.UserId)
                };
            return View(requestViewModel);
        }
    }
}