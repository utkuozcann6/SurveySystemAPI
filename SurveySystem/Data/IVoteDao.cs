using SurveySystem.Models;

namespace SurveySystem.Data
{
    public interface IVoteDao
    {
        Task<bool> HasUserVotedAsync(int userId, int surveyId);
        Task<int> CreateVoteAsync(Vote vote);
    }
}
