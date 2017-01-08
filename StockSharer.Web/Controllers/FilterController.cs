using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;

namespace StockSharer.Web.Controllers
{
    public class FilterController : Controller
    {
        public ActionResult Location(int id)
        {
            return View(new LocationFilterViewModel
                {
                    Locations = RetrieveLocations(id),
                    ActiveLocationId = id
                });
        }

        private static List<Models.Location> RetrieveLocations(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   WITH Descendants as (
                                          SELECT LocationId, ParentLocationId, Name, UrlToken
                                          FROM Location
                                          WHERE LocationId = @LocationId
                                          UNION all
                                          SELECT l.LocationId, l.ParentLocationId, l.Name, l.UrlToken
                                          FROM Location l
                                            join Descendants on l.ParentLocationId = Descendants.LocationId
                                        ), Ancestors as (
                                          SELECT LocationId, ParentLocationId, Name, UrlToken
                                          FROM Location
                                          WHERE LocationId = @LocationId
                                          UNION all
                                          SELECT l.LocationId, l.ParentLocationId, l.Name, l.UrlToken
                                          FROM Location l
                                            join Ancestors on l.LocationId  = Ancestors.ParentLocationId 
                                          )
                                        SELECT LocationId, ParentLocationId, Name, UrlToken
                                        FROM Descendants
                                        WHERE LocationId = @LocationId OR ParentLocationId = @LocationId
                                        UNION all
                                        SELECT LocationId, ParentLocationId, Name, UrlToken
                                        FROM Ancestors
                                        WHERE LocationId <> @LocationId
                                        ORDER BY ParentLocationId, Name";
                return connection.Query<Models.Location>(sql, new {LocationId = id}).ToList();
            }
        }
    }

    public class LocationFilterViewModel
    {
        public List<Models.Location> Locations { get; set; }
        public int ActiveLocationId { get; set; }
    }
}