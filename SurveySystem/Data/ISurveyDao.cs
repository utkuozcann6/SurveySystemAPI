using SurveySystem.Models;

namespace SurveySystem.Data
{
    public interface ISurveyDao
    {
        Task<int> CreateSurveyAsync(Survey survey);
        Task<int> CreateSurveyOptionAsync(SurveyOption option);
        Task<Survey> GetByIdAsync(int id);
        Task<List<Survey>> GetByUserIdAsync(int userId);
        Task<List<SurveyOption>> GetOptionsBySurveyIdAsync(int surveyId);
    }
}
