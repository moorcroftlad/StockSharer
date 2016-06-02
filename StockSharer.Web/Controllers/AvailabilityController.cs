using System;
using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Email;

namespace StockSharer.Web.Controllers
{
    public class AvailabilityController : BaseController
    {
        private readonly RequestRepository _requestRepository;
        private readonly ISendEmail _emailSender;

        public AvailabilityController()
        {
            _requestRepository = new RequestRepository();
            _emailSender = new SesEmailSender();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void SubmitRequest(Guid reference, DateTime endDate)
        {
            _requestRepository.InsertRequest(reference, endDate, User.UserId);
            EmailGameOwner(reference);
        }

        private void EmailGameOwner(Guid reference)
        {
            var gameOwner = _requestRepository.RetrieveGameOwner(reference);
            var bodyText = string.Format("A user has requested to rent {0}, go to http://www.stocksharer.com/settings/requests to accept or reject the request", gameOwner.GameName);
            _emailSender.SendEmail(gameOwner.Email, "New game request", bodyText, bodyText);
        }
    }
}