using SurveySystem.Models;
using SurveySystem.DTOs;

namespace SurveySystem.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<User> GetByIdAsync(int id);
    }
}