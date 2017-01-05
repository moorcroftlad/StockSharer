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
            filter.Town = id;
            var searchResultsViewModel = new SearchResultsViewModel
                {
                    SearchFilter = filter,
                    SearchResults = RetrieveSearchResults(filter)
                };
            return View(searchResultsViewModel);
        }

        private static List<SearchResult> RetrieveSearchResults(SearchFilter filter)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT  g.name GameName, g.ImageUrl, st.Name StockTypeName, p.Name PlatformName, sc.Price, a.Town, ss.Name StockStatusName
                                        FROM    Game g 
                                                INNER JOIN Stock sc on sc.GameId = g.GameId
                                                INNER JOIN Platform p on p.PlatformId = g.PlatformId
                                                INNER JOIN StockType st on st.StockTypeId = sc.StockTypeId
                                                INNER JOIN Store sr on sr.StoreId = sc.StoreId
                                                INNER JOIN Address a on a.AddressId = sr.AddressId
                                                INNER JOIN StockStatus ss on ss.StockStatusId = sc.StockStatusId
                                        WHERE   sr.Active = 1
                                                AND ss.StockStatusId != 3
                                                AND (@Town IS NULL OR a.Town = @Town)
                                                AND (@PlatformId IS NULL OR g.PlatformId = @PlatformId)
                                                AND (@StockTypeId IS NULL OR sc.StockTypeId = @StockTypeId)
                                                AND (@StoreTypeId IS NULL OR sr.StoreTypeId = @StoreTypeId);";
                return connection.Query<SearchResult>(sql, filter).ToList();
            }
        }
    }
}