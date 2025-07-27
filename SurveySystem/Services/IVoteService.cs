using SurveySystem.DTOs;

namespace SurveySystem.Services
{
    public interface IVoteService
    {
        Task VoteAsync(VoteDto voteDto, int userId);
    }
}