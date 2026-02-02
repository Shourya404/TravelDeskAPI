using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TravelDeskAPI.Data;
using TravelDeskAPI.Models;
using TravelDeskAPI.DTOs;

namespace TravelDeskAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TravelDeskDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(TravelDeskDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, object? Data)> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Email == email && u.IsActive)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return (false, "Please enter the correct Email & Password", null);
                }

                if (!VerifyPassword(password, user.Password))
                {
                    return (false, "Please enter the correct Email & Password", null);
                }

                var token = GenerateJwtToken(user.Id, user.Email, user.Role.ToString());

                var loginResponse = new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.ToString(),
                    Token = token
                };

                return (true, "Login successful", loginResponse);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during login: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string firstName, string lastName, string email, string password, string employeeId, string department, string role)
        {
            try
            {
                var existingUser = await _context.Users
                    .Where(u => u.Email == email || u.EmployeeID == employeeId)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return (false, "User with this email or employee ID already exists");
                }

                if (!Enum.TryParse<UserRole>(role, true, out var userRole))
                {
                    return (false, "Invalid role");
                }

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = HashPassword(password),
                    EmployeeID = employeeId,
                    Department = department,
                    Role = userRole,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return (true, "User registered successfully");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred during registration: {ex.Message}");
            }
        }

        public string GenerateJwtToken(int userId, string email, string role)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "TravelDeskAPI";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "TravelDeskApp";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                // Return false if verification fails for any reason (e.g. invalid hash format)
                return false;
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
