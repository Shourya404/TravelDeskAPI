using TravelDeskAPI.Models;

namespace TravelDeskAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TravelDeskDbContext context)
        {
            // Create database if it doesn't exist
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Users.Any())
                return;

            // Seed initial admin user
            var adminPassword = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@traveldesk.com",
                Password = adminPassword,
                EmployeeID = "ADM001",
                Department = "IT",
                Role = UserRole.Admin,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
