using System;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectResult Accept(Guid reference)
        {
            _requestRepository.AcceptRequest(reference);
            return new RedirectResult("/settings/requests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectResult Reject(Guid reference)
        {
            _requestRepository.RejectRequest(reference);
            return new RedirectResult("/settings/requests");
        }
    }
}