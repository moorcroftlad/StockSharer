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
        public ActionResult Index(SearchFilter filter)
        {
            var searchResultsViewModel = new SearchResultsViewModel
                {
                    SearchFilter = filter,
                    SearchResults = RetrieveSearhResults(filter)
                };
            return View(searchResultsViewModel);
        }

        private static List<SearchResult> RetrieveSearhResults(SearchFilter filter)
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
                                                AND ss.StockStatusId != 3";
                return connection.Query<SearchResult>(sql).ToList();
            }
        }
    }
}