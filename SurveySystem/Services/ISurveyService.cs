using SurveySystem.Models;
using SurveySystem.DTOs;

namespace SurveySystem.Services
{
    public interface ISurveyService
    {
        Task<Survey> CreateSurveyAsync(CreateSurveyDto createSurveyDto, int userId);
        Task<Survey> GetSurveyByIdAsync(int id);
        Task<List<Survey>> GetUserSurveysAsync(int userId);
        Task<SurveyResultDto> GetSurveyResultsAsync(int surveyId);
    }
}