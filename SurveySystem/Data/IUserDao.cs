using SurveySystem.Models;

namespace SurveySystem.Data
{
    public interface IUserDao
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(int id);
        Task<int> CreateAsync(User user);
        Task<bool> ExistsAsync(string username, string email);
    }
}
