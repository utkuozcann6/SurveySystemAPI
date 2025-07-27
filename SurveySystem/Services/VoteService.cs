using SurveySystem.Models;
using SurveySystem.DTOs;
using SurveySystem.Data;

namespace SurveySystem.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteDao _voteDao; // vote repository
        private readonly ISurveyDao _surveyDao;

        public VoteService(IVoteDao voteDao, ISurveyDao surveyDao)
        {
            _voteDao = voteDao;
            _surveyDao = surveyDao;
        }

        public async Task VoteAsync(VoteDto voteDto, int userId)
        {
            // Kullanıcı ankete daha önce oy vermiş mi kontrol et
            if (await _voteDao.HasUserVotedAsync(userId, voteDto.SurveyId))
            {
                throw new InvalidOperationException("Daha önce bu ankete oy verildi.");
            }

            // Anketin var olup olmadığını kontrol et
            var survey = await _surveyDao.GetByIdAsync(voteDto.SurveyId);
            if (survey == null)
            {
                throw new ArgumentException("Anket bulunamadı");
            }

            // Anket seçeneğinin geçerli olup olmadığını kontrol et
            var options = await _surveyDao.GetOptionsBySurveyIdAsync(voteDto.SurveyId);
            if (!options.Any(o => o.Id == voteDto.SurveyOptionId))
            {
                throw new ArgumentException("Anket seçeneği geçerli değil");
            }

            var vote = new Vote
            {
                UserId = userId,
                SurveyId = voteDto.SurveyId,
                SurveyOptionId = voteDto.SurveyOptionId,
                VotedAt = DateTime.UtcNow
            };

            await _voteDao.CreateVoteAsync(vote);
        }
    }
}