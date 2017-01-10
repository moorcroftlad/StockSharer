using System;
using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Email;

namespace StockSharer.Web.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly StockSharerRepository _stockSharerRepository;
        private readonly ISendEmail _emailSender;

        public ReservationController()
        {
            _stockSharerRepository = new StockSharerRepository();
            _emailSender = new SesEmailSender();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Reserve(Guid reference, DateTime endDate)
        {
            _stockSharerRepository.SubmitReservation(reference, endDate, User.UserId);
            EmailGameOwner(reference);
        }

        private void EmailGameOwner(Guid reference)
        {
            //var gameOwner = _requestRepository.RetrieveGameOwner(reference);
            //var bodyText = string.Format("A user has requested to rent {0}, go to http://www.stocksharer.com/settings/requests to accept or reject the request", gameOwner.GameName);
            //_emailSender.SendEmail(gameOwner.Email, "New game request", bodyText, bodyText);
        }
    }
}