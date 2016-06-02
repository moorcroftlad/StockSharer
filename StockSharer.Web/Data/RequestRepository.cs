using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.Web.Areas.Settings.Models;

namespace StockSharer.Web.Data
{
    public class RequestRepository
    {
        private readonly string _connectionString;

        public RequestRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public RequestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertRequest(Guid reference, DateTime endDate, int userId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   INSERT INTO Request(GameAvailabilityId, UserId, StartDate, EndDate, Timestamp)
                                        SELECT      GameAvailabilityId, @UserId, @StartDate, @EndDate, @Timestamp
                                        FROM        GameAvailability
                                        WHERE       Reference = @Reference";
                connection.Execute(sql, new
                {
                    Reference = reference,
                    UserId = userId,
                    StartDate = DateTime.Today,
                    EndDate = endDate,
                    Timestamp = DateTime.Now
                });
            }
        }

        public List<GameRequest> RetrieveMyRequests(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   SELECT  r.Reference, g.Name GameName, DateDiff(day, r.StartDate, r.EndDate) Nights, 
                                                (DateDiff(day, r.StartDate, r.EndDate) * ga.PricePerNight) TotalPrice, r.StartDate, 
                                                r.Accepted, r.Rejected, r.Timestamp
                                        FROM    Request r
                                                INNER JOIN GameAvailability ga on ga.GameAvailabilityId = r.GameAvailabilityId
                                                INNER JOIN Game g on g.GameId = ga.GameId
                                        WHERE   ga.UserId = @UserId
                                        ORDER BY Timestamp DESC";
                return connection.Query<GameRequest>(sql, new { UserId = userId }).ToList();
            }
        }

        public void AcceptRequest(Guid reference)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE Request SET Accepted = @TimeNow WHERE Reference = @Reference";
                connection.Execute(sql, new { Reference = reference, TimeNow = DateTime.Now });
            }
        }

        public void RejectRequest(Guid reference)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"UPDATE Request SET Rejected = @TimeNow WHERE Reference = @Reference";
                connection.Execute(sql, new { Reference = reference, TimeNow = DateTime.Now });
            }
        }
    }
}