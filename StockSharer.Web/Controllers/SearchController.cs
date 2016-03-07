using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using StockSharer.Web.Models;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICalulateDistance _distanceCalculator;

        public SearchController()
        {
            _distanceCalculator = new GoogleMapsApiDistanceCalculator();
        }

        public ActionResult Index(string postcode, int? radius = null)
        {
            var searchResults = RetrieveSearchResults();
            var locationResults = searchResults.Where(searchResult => _distanceCalculator.CalculateDistanceBetweenPostcodes(postcode, searchResult.Latitude, searchResult.Longitude) < radius.GetValueOrDefault(1500)).ToList();
            var searchResultsViewModel = new SearchResultsViewModel
            {
                SearchResults = locationResults,
                Postcode = postcode,
                Radius = radius
            };
            return View(searchResultsViewModel);
        }

        private static IEnumerable<SearchResult> RetrieveSearchResults()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT 	g.ImageUrl, a.Name Availability, g.Name GameName, ad.Postcode, g.HostedImageUrl, ad.Latitude, ad.Longitude
                                        FROM 	GameAvailability ga
		                                        INNER JOIN Game g ON g.GameId = ga.GameId
                                                INNER JOIN Availability a ON a.AvailabilityId = ga.AvailabilityId
                                                INNER JOIN [User] u ON u.UserId = ga.UserId
                                                INNER JOIN Address ad ON ad.AddressId = u.AddressId";
                return connection.Query<SearchResult>(sql).ToList();
            }
        }
    }
}