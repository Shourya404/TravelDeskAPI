namespace TravelDeskAPI.Services
{
    public interface IAuthenticationService
    {
        Task<(bool Success, string Message, object? Data)> LoginAsync(string email, string password);
        Task<(bool Success, string Message)> RegisterAsync(string firstName, string lastName, string email, string password, string employeeId, string department, string role);
        string GenerateJwtToken(int userId, string email, string role);
        bool VerifyPassword(string password, string hash);
        string HashPassword(string password);
    }
}
