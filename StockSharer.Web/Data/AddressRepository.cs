using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace StockSharer.Web.Data
{
    public class AddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Address RetrieveAddress(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   SELECT UserId, Line1, Line2, Town, County, Postcode, Latitude, Longitude 
                                        FROM Address 
                                        WHERE UserId = @UserId";
                return connection.Query<Address>(sql, new { UserId = userId }).FirstOrDefault();
            }
        }

        public void UpdateAddress(Address address)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"   merge Address as target
                                        using (values (@UserId))
                                            as source (UserId)
                                            on target.UserId = source.UserId
                                        when matched then
                                            update
                                            SET Line1 = @Line1, Line2 = @Line2, Town = @Town, County = @County, Postcode = @Postcode, Latitude = @Latitude, Longitude = @Longitude
                                        when not matched then
                                            insert (UserId, Line1, Line2, Town, County, Postcode, Latitude, Longitude)
                                            values (@UserId, @Line1, @Line2, @Town, @County, @Postcode, @Latitude, @Longitude);";
                connection.Execute(sql, address);
            }
        }
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public int UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}