using SurveySystem.Models;
using SurveySystem.DTOs;
using SurveySystem.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace SurveySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDao _userDao; //user repository
        private readonly IConfiguration _configuration;

        public UserService(IUserDao userDao, IConfiguration configuration)
        {
            _userDao = userDao;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            // kullanıcı adı ve e-posta kontrolü
            if (await _userDao.ExistsAsync(registerDto.Username, registerDto.Email))
            {
                throw new ArgumentException("Kullanıcı adı veya e-posta zaten mevcut");
            }
            // yeni kullanıcı oluşturma
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCryptNet.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow
            };
            var userId = await _userDao.CreateAsync(user);
            user.Id = userId;
            return GenerateJwtToken(user);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userDao.GetByUsernameAsync(loginDto.Username);
            if (user == null || !BCryptNet.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Kullanıcı adı veya şifre yanlış");
            }
            return GenerateJwtToken(user);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userDao.GetByIdAsync(id);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
