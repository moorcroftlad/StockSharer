﻿using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using StockSharer.Web.Models;

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

        public User RetrieveUser(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT UserId, Forename, Surname, Email FROM [User] WHERE Email = @Email;";
                return connection.Query<User>(sql, new { Email = email }).FirstOrDefault();
            }
        }

        public string RetrievePasswordHash(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT PasswordHash FROM [User] WHERE Email = @Email;";
                return connection.Query<string>(sql, new {Email = email}).FirstOrDefault();
            }
        }

        public int CreateUser(string email, string forename, string surname, string passwordHash)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (UserExists(email))
                {
                    return 0;
                }

                const string sql = @"   INSERT INTO [User] (Email, Forename, Surname, PasswordHash)
                                        VALUES (@Email, @Forename, @Surname, @PasswordHash);
                                        SELECT CAST(SCOPE_IDENTITY() as int)";
                return connection.Query<int>(sql, new { Email = email, Forename = forename, Surname = surname, PasswordHash = passwordHash }).FirstOrDefault();
            }
        }

        private bool UserExists(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                const string sql = @"SELECT Count(*) FROM [User] WHERE Email = @Email;";
                return connection.Query<int>(sql, new { Email = email }).FirstOrDefault() > 0;
            }
        }
    }
}