using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace StockSharer.Web.Data
{
    public class UserAvailabilityRepository
    {
        private readonly string _connectionString;

        public UserAvailabilityRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public UserAvailabilityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<UserAvailability> RetrieveUserAvailabilitys(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   SELECT Day, CONVERT(DATETIME, StartTime) StartTime, CONVERT(DATETIME, EndTime) EndTime
                                        FROM UserAvailability 
                                        WHERE UserId = @UserId";
                return connection.Query<UserAvailability>(sql, new {UserId = userId}).ToList();
            }
        }

        public void UpdateUserAvailabilitys(List<UserAvailability> userAvailabilitys, int userId)
        {
            foreach (var userAvailability in userAvailabilitys)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    const string sql = @"   MERGE INTO UserAvailability AS target
                                            USING (VALUES (@UserId, @Day)) AS source (UserId, Day)
                                                ON  target.UserId = source.UserId AND
                                                    target.Day = source.Day
                                            WHEN MATCHED THEN 
                                                UPDATE SET target.StartTime = @StartTime, target.EndTime = @EndTime
                                            WHEN NOT MATCHED BY TARGET THEN
                                                INSERT (UserId, Day, StartTime, EndTime)
                                                VALUES (@UserId, @Day, @StartTime, @EndTime);";
                    connection.Execute(sql, new
                        {
                            UserId = userId,
                            userAvailability.StartTime,
                            userAvailability.EndTime,
                            userAvailability.Day
                        });
                }
            }

        }
    }

    public class UserAvailability
    {
        public int Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}