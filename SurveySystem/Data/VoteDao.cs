using SurveySystem.Models;
using System.Data;
using Dapper;

namespace SurveySystem.Data
{
    public class VoteDao : IVoteDao 
    {
        //vote repository
        private readonly IDbConnection _connection;

        public VoteDao(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> HasUserVotedAsync(int userId, int surveyId)
        {
            // Kullanıcının ankete daha önce oy verip vermediğini kontrol edin
            const string sql = @"
                SELECT COUNT(*) 
                FROM Votes 
                WHERE UserId = @UserId AND SurveyId = @SurveyId";
            var count = await _connection.QuerySingleAsync<int>(sql, new { UserId = userId, SurveyId = surveyId });
            return count > 0;
        }

        public async Task<int> CreateVoteAsync(Vote vote)
        {
            // Yeni oy ekleme
            const string sql = @"
                INSERT INTO Votes (UserId, SurveyId, SurveyOptionId, VotedAt) 
                VALUES (@UserId, @SurveyId, @SurveyOptionId, @VotedAt);
                SELECT LAST_INSERT_ID();";
            return await _connection.QuerySingleAsync<int>(sql, vote);
        }

        public async Task<List<Vote>> GetVotesBySurveyIdAsync(int surveyId)
        {
            // Belirli bir ankete verilen oyları alma
            const string sql = @"
                SELECT Id, UserId, SurveyId, SurveyOptionId, VotedAt 
                FROM Votes 
                WHERE SurveyId = @SurveyId";
            var votes = await _connection.QueryAsync<Vote>(sql, new { SurveyId = surveyId });
            return votes.ToList();
        }
    }
}
