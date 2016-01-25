using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using StockSharer.GameScraper.Models;

namespace StockSharer.GameScraper
{
    class GameRepository
    {
        public void Insert(List<Game> games)
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
                                            set Name = @Name, ImageUrl = @ImageUrl
                                        when not matched then
                                            insert (PlatformId, Name, ImageUrl)
                                            values (@PlatformId, @Name, @ImageUrl);";
                connection.Execute(sql, games);
            }
        }
    }
}