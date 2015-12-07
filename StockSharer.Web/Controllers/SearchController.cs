using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using MySql.Data.MySqlClient;
using StockSharer.Web.Models;
using StockSharer.Web.ViewModels;

namespace StockSharer.Web.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string q)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                var sql = @"SELECT 	g.ImageUrl, a.Name Availability, g.Name GameName, ad.Postcode
                            FROM 	GameAvailability ga
		                            INNER JOIN Game g ON g.GameId = ga.GameId
                                    INNER JOIN Availability a ON a.AvailabilityId = ga.AvailabilityId
                                    INNER JOIN User u ON u.UserId = ga.UserId
                                    INNER JOIN Address ad ON ad.AddressId = u.AddressId;";

                var searchResultsViewModel = new SearchResultsViewModel
                    {
                        SearchResults = connection.Query<SearchResult>(sql).ToList(),
                        PostCode = "M41PE"
                    };
                return View(searchResultsViewModel);
            }
        }
    }
}