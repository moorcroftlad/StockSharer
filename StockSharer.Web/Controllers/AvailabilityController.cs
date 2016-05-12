using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;

namespace StockSharer.Web.Controllers
{
    public class AvailabilityController : BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void SubmitRequest(Guid reference, DateTime endDate)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   INSERT INTO Request(GameAvailabilityId, UserId, StartDate, EndDate)
                                        SELECT      GameAvailabilityId, @UserId, @StartDate, @EndDate
                                        FROM        GameAvailability
                                        WHERE       Reference = @Reference";
                connection.Execute(sql, new
                    {
                        Reference = reference,
                        User.UserId,
                        StartDate = DateTime.Today,
                        EndDate = endDate
                    });
            }
        }
    }
}