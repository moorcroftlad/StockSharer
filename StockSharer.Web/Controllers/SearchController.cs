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
    public class SearchController : BaseController
    {
        public ActionResult Location(string id, SearchFilter filter)
        {
            /*
            TODO switch to indexed location pages rather than town
            TODO add a search by location page, set it to noindex, nofollow
            */

            var location = RetrieveLocation(id);
            if (location == null)
            {
                return View("LocationNotFound");
            }
            filter.LocationId = location.LocationId;
            var searchResultsViewModel = new LocationSearchResultsViewModel
                {
                    LocationName = location.Name,
                    SearchFilter = filter,
                    SearchResults = RetrieveSearchResults(filter)
                };
            return View(searchResultsViewModel);
        }

        private static Models.Location RetrieveLocation(string urlToken)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT LocationId, Name, UrlToken, ParentLocationId
                                        FROM Location 
                                        WHERE UrlToken = @UrlToken";
                return connection.Query<Models.Location>(sql, new { UrlToken = urlToken }).SingleOrDefault();
            }
        }

        private static List<SearchResult> RetrieveSearchResults(SearchFilter filter)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT  g.name GameName, g.ImageUrl, st.Name StockTypeName, p.Name PlatformName, sc.Price, a.Town TownName, ss.Name StockStatusName, sc.Timestamp DateAdded
                                        FROM    Game g 
                                                INNER JOIN Stock sc on sc.GameId = g.GameId
                                                INNER JOIN Platform p on p.PlatformId = g.PlatformId
                                                INNER JOIN StockType st on st.StockTypeId = sc.StockTypeId
                                                INNER JOIN Store sr on sr.StoreId = sc.StoreId
                                                INNER JOIN Address a on a.AddressId = sr.AddressId
                                                INNER JOIN Location l on a.LocationId = l.LocationId
                                                INNER JOIN StockStatus ss on ss.StockStatusId = sc.StockStatusId
                                        WHERE   sr.Active = 1
                                                AND ss.StockStatusId != 3
                                                AND (@LocationId IS NULL OR a.LocationId = @LocationId)
                                                AND (@PlatformId IS NULL OR g.PlatformId = @PlatformId)
                                                AND (@StockStatusId IS NULL OR sc.StockStatusId = @StockStatusId)
                                                AND (@StoreTypeId IS NULL OR sr.StoreTypeId = @StoreTypeId)
                                        ORDER BY sc.Timestamp DESC";
                return connection.Query<SearchResult>(sql, filter).ToList();
            }
        }
    }
}