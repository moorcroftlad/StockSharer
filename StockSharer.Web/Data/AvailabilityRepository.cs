using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.Web.Models;

namespace StockSharer.Web.Data
{
    public class AvailabilityRepository
    {
        private readonly string _connectionString;

        public AvailabilityRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public AvailabilityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<GameAvailability> RetrieveMyGames(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   select g.GameId, g.Name GameName, g.HostedImageUrl, p.Name PlatformName, p.PlatformId, ga.DateAdded, ga.AvailabilityId, a.Name AvailabilityName
                                        from gameavailability ga
                                        inner join Game g on ga.GameId = g.GameId
                                        inner join Availability a on a.AvailabilityId = ga.AvailabilityId
                                        inner join Platform p on p.PlatformId = g.PlatformId
                                        where ga.UserId = @UserId";
                return connection.Query<GameAvailability>(sql, new { UserId = userId }).ToList();
            }
        }

        public GameAvailability RetrieveGameAvailability(int gameId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   select g.GameId, g.Name GameName, g.HostedImageUrl, p.Name PlatformName, p.PlatformId, ga.DateAdded, ga.AvailabilityId, a.Name AvailabilityName
                                        from gameavailability ga
                                        inner join Game g on ga.GameId = g.GameId
                                        inner join Availability a on a.AvailabilityId = ga.AvailabilityId
                                        inner join Platform p on p.PlatformId = g.PlatformId
                                        where ga.UserId = @UserId and ga.GameId = @GameId";
                return connection.Query<GameAvailability>(sql, new { UserId = userId, GameId = gameId }).FirstOrDefault();
            }
        }

        public GameAvailability AddGameAvailability(int gameId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   INSERT INTO GameAvailability (GameId, AvailabilityId, UserId, DateAdded)
                                        VALUES (@GameId, @AvailabilityId, @UserId, @DateAdded)";
                connection.Execute(sql, new
                {
                    GameId = gameId,
                    AvailabilityId = 1,
                    UserId = userId,
                    DateAdded = DateTime.Now
                });
            }
            return RetrieveGameAvailability(gameId, userId);
        }
    }
}