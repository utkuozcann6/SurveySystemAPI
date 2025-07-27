using SurveySystem.Models;
using System.Data;
using Dapper;

namespace SurveySystem.Data
{
    public class UserDao : IUserDao
    {
        // User repository
        private readonly IDbConnection _connection;

        public UserDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            // Kullanıcı adını kullanarak kullanıcıyı alma
            const string sql = @"
                SELECT Id, Username, Email, Password, CreatedAt 
                FROM Users 
                WHERE Username = @Username";
            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<User> GetByIdAsync(int id)
        {
            // Kullanıcı ID'sine göre kullanıcıyı alma
            const string sql = @"
                SELECT Id, Username, Email, Password, CreatedAt 
                FROM Users 
                WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(User user)
        {
            // Yeni kullanıcı oluşturma
            const string sql = @"
                INSERT INTO Users (Username, Email, Password, CreatedAt) 
                VALUES (@Username, @Email, @Password, @CreatedAt);
                SELECT LAST_INSERT_ID();";
            return await _connection.QuerySingleAsync<int>(sql, user);
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            // Kullanıcı adının veya e-posta adresinin var olup olmadığını kontrol etme
            const string sql = @"
                SELECT COUNT(*) 
                FROM Users 
                WHERE Username = @Username OR Email = @Email";
            var count = await _connection.QuerySingleAsync<int>(sql, new { Username = username, Email = email });
            return count > 0;
        }
    }
}

