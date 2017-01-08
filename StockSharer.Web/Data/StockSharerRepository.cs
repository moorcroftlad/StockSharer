using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.Web.Models;

namespace StockSharer.Web.Data
{
    public class StockSharerRepository
    {
        private readonly string _connectionString;

        public StockSharerRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public StockSharerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //TODO: Cache these results for a period of time
        public List<SearchResult> RetrieveSearchResults(SearchFilter filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   WITH Descendants as (
                                            SELECT SearchLocationId, ParentSearchLocationId
                                            FROM SearchLocation
                                            WHERE SearchLocationId = @SearchLocationId
                                            UNION all
                                            SELECT l.SearchLocationId, l.ParentSearchLocationId
                                            FROM SearchLocation l
                                            join Descendants on l.ParentSearchLocationId = Descendants.SearchLocationId
                                        )
                                        SELECT  g.name GameName, g.ImageUrl, st.Name StockTypeName, p.Name PlatformName, sc.Price, a.Town TownName, ss.Name StockStatusName, sc.Timestamp DateAdded
                                        FROM    Game g 
                                                INNER JOIN Stock sc on sc.GameId = g.GameId
                                                INNER JOIN Platform p on p.PlatformId = g.PlatformId
                                                INNER JOIN StockType st on st.StockTypeId = sc.StockTypeId
                                                INNER JOIN Store sr on sr.StoreId = sc.StoreId
                                                INNER JOIN Address a on a.AddressId = sr.AddressId
                                                INNER JOIN StockStatus ss on ss.StockStatusId = sc.StockStatusId
                                        WHERE   sr.Active = 1
                                                AND ss.StockStatusId != 3
                                                AND (@SearchLocationId IS NULL OR a.SearchLocationId in 
		                                        (
			                                        SELECT SearchLocationId 
			                                        FROM Descendants
		                                        ))
                                                AND (@PlatformId IS NULL OR g.PlatformId = @PlatformId)
                                                AND (@StockStatusId IS NULL OR sc.StockStatusId = @StockStatusId)
                                                AND (@StoreTypeId IS NULL OR sr.StoreTypeId = @StoreTypeId)";
                return connection.Query<SearchResult>(sql, filter).ToList();
            }
        }

        public SearchLocation RetrieveSearchLocation(string urlToken)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   SELECT SearchLocationId, Name, UrlToken, ParentSearchLocationId
                                        FROM SearchLocation 
                                        WHERE UrlToken = @UrlToken";
                return connection.Query<SearchLocation>(sql, new { UrlToken = urlToken }).SingleOrDefault();
            }
        }

        public List<SearchLocation> RetrieveSearchLocations(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString()))
            {
                const string sql = @"   WITH Descendants as (
                                          SELECT SearchLocationId, ParentSearchLocationId, Name, UrlToken
                                          FROM SearchLocation
                                          WHERE SearchLocationId = @SearchLocationId
                                          UNION all
                                          SELECT l.SearchLocationId, l.ParentSearchLocationId, l.Name, l.UrlToken
                                          FROM SearchLocation l
                                            join Descendants on l.ParentSearchLocationId = Descendants.SearchLocationId
                                        ), Ancestors as (
                                          SELECT SearchLocationId, ParentSearchLocationId, Name, UrlToken
                                          FROM SearchLocation
                                          WHERE SearchLocationId = @SearchLocationId
                                          UNION all
                                          SELECT l.SearchLocationId, l.ParentSearchLocationId, l.Name, l.UrlToken
                                          FROM SearchLocation l
                                            join Ancestors on l.SearchLocationId  = Ancestors.ParentSearchLocationId 
                                          )
                                        SELECT SearchLocationId, ParentSearchLocationId, Name, UrlToken
                                        FROM Descendants
                                        WHERE SearchLocationId = @SearchLocationId OR ParentSearchLocationId = @SearchLocationId
                                        UNION all
                                        SELECT SearchLocationId, ParentSearchLocationId, Name, UrlToken
                                        FROM Ancestors
                                        WHERE SearchLocationId <> @SearchLocationId
                                        ORDER BY ParentSearchLocationId, Name";
                return connection.Query<SearchLocation>(sql, new { SearchLocationId = id }).ToList();
            }
        }
    }
}