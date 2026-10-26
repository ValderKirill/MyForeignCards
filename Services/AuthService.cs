using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(ILogger<AuthService> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> GetUserAsync(UserRequest userData)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Login == userData.Login);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                userData.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Ошибка входа: {UserName}", userData.Login);
            }
            else
            {
                _logger.LogInformation("Пользователь {UserName} найден", user.Login);
            }

            return user;
        }

        public async Task<UserResponse?> AddUserAsync(User newUser)
        {
            var isNameUnique = _context.Users
                .AsNoTracking()
                .AnyAsync(user => user.Login == newUser.Login);

            if(await isNameUnique)
            {
                _logger.LogDebug("User with login {Login} is already exist",
                    newUser.Login);

                return null;
            }

            newUser.Password = _passwordHasher.HashPassword(newUser, newUser.Password);

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
                Login = newUser.Login
            };

            return userResponse;
        }
    }
}