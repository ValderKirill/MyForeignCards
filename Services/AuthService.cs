using Microsoft.EntityFrameworkCore;
using MyForeignCards.Data;
using MyForeignCards.DTOs;
using MyForeignCards.Entities;

namespace MyForeignCards.Services
{
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ApplicationContext _context;

        public AuthService(ILogger<AuthService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> GetUserAsync(UserRequest userData)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Login == userData.Login && user.Password == userData.Password);

            if (user == null)
            {
                _logger.LogWarning("Ошибка входа: {UserName}", userData.Login);
            }
            else
            {
                _logger.LogInformation("Пользователь {UserName} найден", user.Login);
            }

            return user;
        }

        public async Task<UserResponse> AddUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {Login} added", 
                newUser.Login);

            _logger.LogDebug("User {Login} added. Id: {Id}",
                newUser.Login,
                newUser.Id);

            var userResponse = new UserResponse()
            {
                Id = newUser.Id,
                Login = newUser.Login,
                Password = newUser.Password
            };

            return userResponse;
        }
    }
}