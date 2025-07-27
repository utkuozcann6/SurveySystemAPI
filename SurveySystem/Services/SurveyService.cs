using SurveySystem.Models;
using SurveySystem.DTOs;
using SurveySystem.Data;

namespace SurveySystem.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly ISurveyDao _surveyDao; // survey repository

        public SurveyService(ISurveyDao surveyDao)
        {
            _surveyDao = surveyDao;
        }

        public async Task<Survey> CreateSurveyAsync(CreateSurveyDto createSurveyDto, int userId)
        {
            // anket seçeneklerini kontrol et
            if (createSurveyDto.Options.Count < 2 || createSurveyDto.Options.Count > 5)
            {
                throw new ArgumentException("Anket 2 ile 5 seçenek arasında olmalıdır.");
            }

            var survey = new Survey
            {
                Title = createSurveyDto.Title,
                Description = createSurveyDto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var surveyId = await _surveyDao.CreateSurveyAsync(survey);
            survey.Id = surveyId;

            // yeni anket için seçenekleri ekle
            foreach (var optionText in createSurveyDto.Options)
            {
                var option = new SurveyOption
                {
                    SurveyId = surveyId,
                    OptionText = optionText
                };

                var optionId = await _surveyDao.CreateSurveyOptionAsync(option);
                option.Id = optionId;
                survey.Options.Add(option);
            }

            return survey;
        }

        public async Task<Survey> GetSurveyByIdAsync(int id)
        {
            var survey = await _surveyDao.GetByIdAsync(id);
            if (survey != null)
            {
                survey.Options = await _surveyDao.GetOptionsBySurveyIdAsync(id);
            }
            return survey;
        }

        public async Task<List<Survey>> GetUserSurveysAsync(int userId)
        {
            var surveys = await _surveyDao.GetByUserIdAsync(userId);
            foreach (var survey in surveys)
            {
                survey.Options = await _surveyDao.GetOptionsBySurveyIdAsync(survey.Id);
            }
            return surveys;
        }

        public async Task<SurveyResultDto> GetSurveyResultsAsync(int surveyId)
        {
            // Anketin var olup olmadığını kontrol et
            var survey = await GetSurveyByIdAsync(surveyId);
            if (survey == null)
            {
                throw new ArgumentException("Anket bulunamadı");
            }

            var totalVotes = survey.Options.Sum(o => o.VoteCount);

            var result = new SurveyResultDto
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                TotalVotes = totalVotes,
                Options = survey.Options.Select(o => new OptionResultDto
                {
                    Id = o.Id,
                    OptionText = o.OptionText,
                    VoteCount = o.VoteCount,
                    Percentage = totalVotes > 0 ? (double)o.VoteCount / totalVotes * 100 : 0
                }).ToList()
            };

            return result;
        }
    }
}