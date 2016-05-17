using System;
using System.Web.Mvc;
using StockSharer.Web.Data;

namespace StockSharer.Web.Controllers
{
    public class AvailabilityController : BaseController
    {
        private readonly RequestRepository _requestRepository;

        public AvailabilityController()
        {
            _requestRepository = new RequestRepository();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void SubmitRequest(Guid reference, DateTime endDate)
        {
            _requestRepository.InsertRequest(reference, endDate, User.UserId);
        }
    }
}