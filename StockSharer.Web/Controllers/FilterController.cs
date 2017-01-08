using System.Collections.Generic;
using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Models;

namespace StockSharer.Web.Controllers
{
    //TODO: Cache these results for a period of time
    public class FilterController : Controller
    {
        private readonly StockSharerRepository _stockSharerRepository = new StockSharerRepository();

        public ActionResult SearchLocation(int id)
        {
            return View(new SearchLocationFilterViewModel
                {
                    SearchLocations = _stockSharerRepository.RetrieveSearchLocations(id),
                    ActiveSearchLocationId = id
                });
        }
    }

    public class SearchLocationFilterViewModel
    {
        public List<SearchLocation> SearchLocations { get; set; }
        public int ActiveSearchLocationId { get; set; }
    }
}