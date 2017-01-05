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
        public ActionResult Town(string id, SearchFilter filter)
        {
            var town = RetrieveTown(id);
            if (town == null)
            {
                return View("TownNotFound");
            }
            filter.TownId = town.TownId;
            var searchResultsViewModel = new TownSearchResultsViewModel
                {
                    Town = town.Name,
                    SearchFilter = filter,
                    SearchResults = RetrieveSearchResults(filter)
                };
            return View(searchResultsViewModel);
        }

        private static Town RetrieveTown(string urlToken)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT TownId, Name, UrlToken
                                        FROM Town 
                                        WHERE UrlToken = @UrlToken";
                return connection.Query<Town>(sql, new { UrlToken = urlToken }).SingleOrDefault();
            }
        }

        private static List<SearchResult> RetrieveSearchResults(SearchFilter filter)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT  g.name GameName, g.ImageUrl, st.Name StockTypeName, p.Name PlatformName, sc.Price, t.Name TownName, ss.Name StockStatusName, sc.Timestamp DateAdded
                                        FROM    Game g 
                                                INNER JOIN Stock sc on sc.GameId = g.GameId
                                                INNER JOIN Platform p on p.PlatformId = g.PlatformId
                                                INNER JOIN StockType st on st.StockTypeId = sc.StockTypeId
                                                INNER JOIN Store sr on sr.StoreId = sc.StoreId
                                                INNER JOIN Address a on a.AddressId = sr.AddressId
                                                INNER JOIN Town t on a.TownId = t.TownId
                                                INNER JOIN StockStatus ss on ss.StockStatusId = sc.StockStatusId
                                        WHERE   sr.Active = 1
                                                AND ss.StockStatusId != 3
                                                AND (@TownId IS NULL OR a.TownId = @TownId)
                                                AND (@PlatformId IS NULL OR g.PlatformId = @PlatformId)
                                                AND (@StockTypeId IS NULL OR sc.StockTypeId = @StockTypeId)
                                                AND (@StoreTypeId IS NULL OR sr.StoreTypeId = @StoreTypeId)
                                        ORDER BY sc.Timestamp DESC";
                return connection.Query<SearchResult>(sql, filter).ToList();
            }
        }
    }
}