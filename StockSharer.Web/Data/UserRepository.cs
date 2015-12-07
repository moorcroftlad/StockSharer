using System.Configuration;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace StockSharer.Web.Data
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

        public int CreateUser(string email, string passwordHash)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                if (UserExists(email))
                {
                    return 0;
                }

                const string sql = @"   INSERT INTO User (Email, PasswordHash)
                                        VALUES (@Email, @PasswordHash);
                                        SELECT LAST_INSERT_ID();";
                return connection.Query<int>(sql, new { Email = email, PasswordHash = passwordHash }).FirstOrDefault();
            }
        }

        private bool UserExists(string email)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                const string sql = @"SELECT Count(*) FROM User WHERE Email = @Email;";
                return connection.Query<int>(sql, new { Email = email }).FirstOrDefault() > 0;
            }
        }
    }
}