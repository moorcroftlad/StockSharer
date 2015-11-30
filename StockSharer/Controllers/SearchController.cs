using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using MySql.Data.MySqlClient;

namespace StockSharer.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string q)
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                var sql = "SELECT FirstNumber FROM TestTable";
                var number = connection.Query<int>(sql).FirstOrDefault();
                var searchResultsViewModel = new SearchResultsViewModel
                    {
                        Number = number
                    };
                return View(searchResultsViewModel);
            }
        }
    }

    public class SearchResultsViewModel
    {
        public int Number { get; set; }
    }
}