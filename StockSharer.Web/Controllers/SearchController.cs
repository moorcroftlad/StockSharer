using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using StockSharer.Web.Data;
using StockSharer.Web.Location;
using StockSharer.Web.Models;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class SearchController : Controller
    {
        private const int DefaultRadius = 1500;
        private readonly ICalulateLocation _locationCalculator;
        private readonly UserRepository _userRepository;
        private readonly DistanceCalculator _distanceCalculator;

        public SearchController()
        {
            _locationCalculator = new GoogleMapsApiLocationCalculator();
            _userRepository = new UserRepository();
            _distanceCalculator = new DistanceCalculator();
        }

        public ActionResult Index(string postcode, int? radius = null)
        {
            var searchLocation = _locationCalculator.CalculateLocation(postcode);
            var searchResults = new List<SearchResult>();
            if (searchLocation != null)
            {
                var userResults = RetrieveUsersInArea(searchLocation, radius.GetValueOrDefault(DefaultRadius));
                searchResults = RetrieveSearchResults(userResults);
            }
            var searchResultsViewModel = new SearchResultsViewModel
                {
                    SearchResults = searchResults.OrderBy(x => x.Distance).ToList(),
                    Postcode = postcode,
                    Radius = radius
                };
            return View(searchResultsViewModel);
        }

        private List<UserResult> RetrieveUsersInArea(Location.Location searchLocation, int radius)
        {
            var users = _userRepository.RetrieveAllActiveUsers();
            var usersInArea = new List<UserResult>();
            foreach (var user in users)
            {
                var location1 = new Location.Location(user.Latitude, user.Longitude);
                var location2 = new Location.Location(searchLocation.Latitude, searchLocation.Longitude);
                var distanceBetweenLocations = _distanceCalculator.CalculateDistanceBetweenLocations(location1, location2);
                if (distanceBetweenLocations < radius)
                {
                    usersInArea.Add(new UserResult
                        {
                            UserId = user.UserId,
                            Distance = distanceBetweenLocations
                        });
                }
            }
            return usersInArea;
        }

        private static List<SearchResult> RetrieveSearchResults(List<UserResult> userResults)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT 	a.Name Availability, g.Name GameName, g.HostedImageUrl, ga.UserId
                                        FROM 	GameAvailability ga
		                                        INNER JOIN Game g ON g.GameId = ga.GameId
                                                INNER JOIN Availability a ON a.AvailabilityId = ga.AvailabilityId
                                        WHERE   ga.UserId in @UserIds";
                var searchResults = connection.Query<SearchResult>(sql, new {UserIds = userResults.Select(x => x.UserId)}).ToList();
                foreach (var searchResult in searchResults)
                {
                    searchResult.Distance = userResults.Single(x => x.UserId == searchResult.UserId).Distance;
                }
                return searchResults;
            }
        }
    }

    public class UserResult
    {
        public int UserId { get; set; }
        public int Distance { get; set; }
    }
}