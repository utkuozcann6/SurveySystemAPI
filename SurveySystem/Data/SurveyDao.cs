using SurveySystem.Models;
using System.Data;
using Dapper;

namespace SurveySystem.Data
{
    public class SurveyDao : ISurveyDao
    {
        // Survey repository
        private readonly IDbConnection _connection;

        public SurveyDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateSurveyAsync(Survey survey)
        {
            // yeni anket ekleme
            const string sql = @"
                INSERT INTO Surveys (Title, Description, UserId, CreatedAt) 
                VALUES (@Title, @Description, @UserId, @CreatedAt);
                SELECT LAST_INSERT_ID();";
            return await _connection.QuerySingleAsync<int>(sql, survey);
        }

        public async Task<int> CreateSurveyOptionAsync(SurveyOption option)
        {
            // yeni anket seçeneği ekleme
            const string sql = @"
                INSERT INTO SurveyOptions (SurveyId, OptionText) 
                VALUES (@SurveyId, @OptionText);
                SELECT LAST_INSERT_ID();";
            return await _connection.QuerySingleAsync<int>(sql, option);
        }

        public async Task<Survey> GetByIdAsync(int id)
        {
            // anketi ID ile alma
            const string sql = @"
                SELECT Id, Title, Description, UserId, CreatedAt 
                FROM Surveys 
                WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Survey>(sql, new { Id = id });
        }

        public async Task<List<Survey>> GetByUserIdAsync(int userId)
        {
            // kullanıcının anketlerini alma
            const string sql = @"
                SELECT Id, Title, Description, UserId, CreatedAt 
                FROM Surveys 
                WHERE UserId = @UserId 
                ORDER BY CreatedAt DESC";
            var surveys = await _connection.QueryAsync<Survey>(sql, new { UserId = userId });
            return surveys.ToList();
        }

        public async Task<List<SurveyOption>> GetOptionsBySurveyIdAsync(int surveyId)
        {
            // anket ID'sine göre seçenekleri alma
            const string sql = @"
                SELECT so.Id, so.SurveyId, so.OptionText, 
                       COALESCE(v.VoteCount, 0) as VoteCount
                FROM SurveyOptions so
                LEFT JOIN (
                    SELECT SurveyOptionId, COUNT(*) as VoteCount
                    FROM Votes
                    GROUP BY SurveyOptionId
                ) v ON so.Id = v.SurveyOptionId
                WHERE so.SurveyId = @SurveyId";
            var options = await _connection.QueryAsync<SurveyOption>(sql, new { SurveyId = surveyId });
            return options.ToList();
        }
    }
}
