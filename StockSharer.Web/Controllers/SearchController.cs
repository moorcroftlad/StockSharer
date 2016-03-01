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
        public ActionResult Index(string postcode, int? radius = null)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT 	g.ImageUrl, a.Name Availability, g.Name GameName, ad.Postcode, g.HostedImageUrl
                                        FROM 	GameAvailability ga
		                                        INNER JOIN Game g ON g.GameId = ga.GameId
                                                INNER JOIN Availability a ON a.AvailabilityId = ga.AvailabilityId
                                                INNER JOIN [User] u ON u.UserId = ga.UserId
                                                INNER JOIN Address ad ON ad.AddressId = u.AddressId";

                var searchResultsViewModel = new SearchResultsViewModel
                    {
                        SearchResults = connection.Query<SearchResult>(sql).ToList(),
                        Postcode = postcode,
                        Radius = radius
                    };
                return View(searchResultsViewModel);
            }
        }
    }
}