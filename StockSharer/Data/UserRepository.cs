using System.Configuration;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace StockSharer.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockSharerDatabase"].ToString();
        }

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string RetrievePasswordHash(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                const string sql = @"SELECT PasswordHash FROM User WHERE Email = @Email;";
                return connection.Query<string>(sql, new {Email = email}).FirstOrDefault();
            }
        }
    }
}