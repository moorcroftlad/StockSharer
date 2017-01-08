using System.Web.Mvc;
using StockSharer.Web.Data;
using StockSharer.Web.Models;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly StockSharerRepository _stockSharerRepository = new StockSharerRepository();

        public ActionResult Location(string id, SearchFilter filter)
        {
            /*
            TODO switch to indexed location pages rather than town
            TODO add a search by location page, set it to noindex, nofollow
            */

            var searchLocation = _stockSharerRepository.RetrieveSearchLocation(id);
            if (searchLocation == null)
            {
                return View("LocationNotFound");
            }
            filter.SearchLocationId = searchLocation.SearchLocationId;
            var searchResultsViewModel = new LocationSearchResultsViewModel
                {
                    SearchLocation = searchLocation,
                    SearchFilter = filter,
                    SearchResults = _stockSharerRepository.RetrieveSearchResults(filter)
                };
            return View(searchResultsViewModel);
        }
    }
}