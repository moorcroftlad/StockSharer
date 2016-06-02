using System;
using System.Web.Mvc;
using StockSharer.Web.Areas.Settings.Models.ViewModels;
using StockSharer.Web.Data;
using StockSharer.Web.Email;

namespace StockSharer.Web.Areas.Settings.Controllers
{
    public class RequestsController : BaseSettingsController
    {
        private readonly RequestRepository _requestRepository;
        private readonly ISendEmail _emailSender;

        public RequestsController()
        {
            _requestRepository = new RequestRepository();
            _emailSender = new SesEmailSender();
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
            var gameRequester = _requestRepository.RetrieveGameRequester(reference);
            var bodyText = string.Format("The owner of {0} has accepted your request, go to http://www.stocksharer.com/settings/requests to view their contact details to arrange collection", gameRequester.GameName);
            _emailSender.SendEmail(gameRequester.Email, "Game request accepted", bodyText, bodyText);
            return new RedirectResult("/settings/requests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectResult Reject(Guid reference)
        {
            _requestRepository.RejectRequest(reference);
            var gameRequester = _requestRepository.RetrieveGameRequester(reference);
            var bodyText = string.Format("The owner of {0} has rejected your request, go to http://www.stocksharer.com/search to view other games available", gameRequester.GameName);
            _emailSender.SendEmail(gameRequester.Email, "Game request rejected", bodyText, bodyText);
            return new RedirectResult("/settings/requests");
        }
    }
}