using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.GameScraper.Models;

namespace StockSharer.GameScraper
{
    class GameRepository
    {
        public void MergeGames(List<Game> games)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   merge Game as target
                                        using (values (@PlatformId, @Name))
                                            as source (PlatformId, Name)
                                            on target.Name = source.Name AND
                                               target.PlatformId = source.PlatformId
                                        when matched then
                                            update
                                            set Name = @Name, ImageUrl = @ImageUrl, HostedImageUrl = @HostedImageUrl
                                        when not matched then
                                            insert (PlatformId, Name, ImageUrl, HostedImageUrl)
                                            values (@PlatformId, @Name, @ImageUrl, @HostedImageUrl);";
                connection.Execute(sql, games);
            }
        }

        public List<Game> RetrieveImagesToSave()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT GameId, ImageUrl, HostedImageUrl
                                        FROM Game
                                        WHERE HostedImageUrl IS NULL;";
                return connection.Query<Game>(sql).ToList();
            }
        }

        public void UpdateImages(List<Game> games)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   UPDATE Game Set HostedImageUrl = @HostedImageUrl WHERE GameId = @GameId";
                connection.Execute(sql, games);
            }
        }
    }
}