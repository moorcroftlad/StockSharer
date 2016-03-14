using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using StockSharer.Web.Location;
using StockSharer.Web.Models;

namespace StockSharer.Web.Controllers
{
    public class PostcodeController : Controller
    {
        private readonly GoogleMapsApiLocationCalculator _locationCalculator;

        public PostcodeController()
        {
            _locationCalculator = new GoogleMapsApiLocationCalculator();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(string postcode)
        {
            var location = _locationCalculator.CalculateLocation(postcode.Replace(" ", "").ToUpper());
            if (location == null)
            {
                return Json(new PostcodeNotFoundResponse());
            }
            Response.Cookies.Add(new HttpCookie("GeoLocation", JsonConvert.SerializeObject(location))
                {
                    Expires = DateTime.Now.AddYears(10)
                });
            return Json(new PostcodeFoundResponse());
        }
    }
}