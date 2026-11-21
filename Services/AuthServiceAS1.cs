using InternLink.Api.Models;
using InternLink.Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InternLink.Api.Services
{
    // IAuthServiceAS1-এর আসল implementation — password hash, email duplicate check, JWT token তৈরি — সব business logic এখানে
    public class AuthServiceAS1 : IAuthServiceAS1
    {
        private readonly IUserRepositoryUR1 _userRepository;
        private readonly IConfiguration _configuration;

        // Constructor Injection — Repository আর Configuration (appsettings.json থেকে JWT secret পড়ার জন্য) দুটোই inject হচ্ছে
        public AuthServiceAS1(IUserRepositoryUR1 userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(string name, string email, string password, UserRole role)
        {
            // একই email দিয়ে আগে থেকে user আছে কিনা check করা — duplicate registration ঠেকাতে
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("এই email দিয়ে আগে থেকেই একটা account আছে");
            }

            // BCrypt দিয়ে password hash করা — কখনো plain text password database-এ রাখা হবে না
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            // Registration সফল হলে সাথে সাথে login-ও করিয়ে দেওয়া হচ্ছে — তাই token generate করে return করছি
            return GenerateJwtToken(newUser);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            // User না পাওয়া গেলে, অথবা password match না করলে — null return (login fail)
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        // JWT token তৈরি করার আসল logic — user-এর তথ্য (Id, Email, Role) token-এর ভেতরে "claims" হিসেবে বসানো হয়
        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"]!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}