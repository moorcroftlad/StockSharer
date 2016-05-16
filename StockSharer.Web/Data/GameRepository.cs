using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.Web.Models;

namespace StockSharer.Web.Data
{
    public class GameRepository
    {
        private readonly string _connectionString;

        public GameRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public GameRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddGameResult> SearchForGames(string name, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   select top 10 g.Name GameName, g.GameId, HostedImageUrl, p.Name PlatformName, 
                                        CASE WHEN EXISTS (select GameId from GameAvailability where GameId = g.GameId AND UserId = @UserId) THEN 'True' ELSE 'False' END AS OwnsGame
                                        from Game g
                                        inner join Platform p on p.PlatformId = g.PlatformId
                                        where g.Name like @SearchTerm
                                        order by g.Name";
                return connection.Query<AddGameResult>(sql, new
                    {
                        SearchTerm = name == null ? null : "%" + name + "%",
                        UserId = userId
                    }).ToList();
            }
        }
    }
}