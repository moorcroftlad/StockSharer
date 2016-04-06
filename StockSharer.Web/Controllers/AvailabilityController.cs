using System;
using System.Web.Mvc;

namespace StockSharer.Web.Controllers
{
    public class AvailabilityController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void SubmitRequestIndex(int gameAvailabilityId, DateTime endDate)
        {
            //TODO submit request
        }
    }
}