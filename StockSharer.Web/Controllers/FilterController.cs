using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Models;

namespace StockSharer.Web.Controllers
{
    public class FilterController : Controller
    {
        private readonly StockSharerRepository _stockSharerRepository = new StockSharerRepository();

        public ActionResult SearchLocation(int id, SearchFilter searchFilter)
        {
            var searchLocations = _stockSharerRepository.RetrieveSearchLocations(id);
            var searchLocationItems = searchLocations.Select(searchLocation =>
                                                             new SearchLocationItem
                                                                 {
                                                                     SearchLocation = searchLocation,
                                                                     NumberOfResults = _stockSharerRepository.RetrieveSearchResults(
                                                                         new SearchFilter
                                                                             {
                                                                                 PlatformId = searchFilter.PlatformId,
                                                                                 StockStatusId = searchFilter.StockStatusId,
                                                                                 StoreTypeId = searchFilter.StoreTypeId,
                                                                                 SearchLocationId = searchLocation.SearchLocationId
                                                                             }).Count
                                                                 }).ToList();
            return View(new SearchLocationFilterViewModel
                {
                    SearchLocations = searchLocationItems,
                    ActiveSearchLocationId = id
                });
        }
    }

    public class SearchLocationFilterViewModel
    {
        public List<SearchLocationItem> SearchLocations { get; set; }
        public int ActiveSearchLocationId { get; set; }
    }
}