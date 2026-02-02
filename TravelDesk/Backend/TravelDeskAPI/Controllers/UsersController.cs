using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDeskAPI.Data;
using TravelDeskAPI.DTOs;
using TravelDeskAPI.Models;

namespace TravelDeskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly TravelDeskDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(TravelDeskDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("total")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalUsers()
        {
            try
            {
                var count = await _context.Users.CountAsync();
                return Ok(new { totalUsers = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total users");
                return StatusCode(500, new { message = "Error retrieving total users" });
            }
        }

        [HttpGet("grid")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserGrid([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var validPageSizes = new[] { 20, 50, 100 };
                if (!validPageSizes.Contains(pageSize))
                    pageSize = 20;

                var users = await _context.Users
                    .Where(u => u.IsActive)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.EmployeeID,
                        u.Department,
                        Role = u.Role.ToString(),
                        u.ManagerName
                    })
                    .ToListAsync();

                var totalCount = await _context.Users.CountAsync(u => u.IsActive);

                return Ok(new
                {
                    data = users,
                    totalCount,
                    pageNumber,
                    pageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user grid");
                return StatusCode(500, new { message = "Error retrieving users" });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var userDto = new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmployeeID = user.EmployeeID,
                    Department = user.Department,
                    Role = user.Role.ToString(),
                    ManagerName = user.ManagerName,
                    IsActive = user.IsActive
                };

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id");
                return StatusCode(500, new { message = "Error retrieving user" });
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var existingUser = await _context.Users
                    .Where(u => u.Email == request.Email || u.EmployeeID == request.EmployeeID)
                    .FirstOrDefaultAsync();

                if (existingUser != null)
                    return BadRequest(new { message = "User with this email or employee ID already exists" });

                if (!Enum.TryParse<UserRole>(request.Role, true, out var userRole))
                    return BadRequest(new { message = "Invalid role" });

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    EmployeeID = request.EmployeeID,
                    Department = request.Department,
                    Role = userRole,
                    ManagerName = request.ManagerName,
                    ManagerId = request.ManagerId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User added: {user.Email}");
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new { message = "User added successfully", userId = user.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user");
                return StatusCode(500, new { message = "Error adding user" });
            }
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                if (!Enum.TryParse<UserRole>(request.Role, true, out var userRole))
                    return BadRequest(new { message = "Invalid role" });

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Department = request.Department;
                user.Role = userRole;
                user.ManagerName = request.ManagerName;
                user.ManagerId = request.ManagerId;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User edited: {user.Email}");
                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing user");
                return StatusCode(500, new { message = "Error updating user" });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                user.IsActive = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User deleted: {user.Email}");
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new { message = "Error deleting user" });
            }
        }

        [HttpPut("assign-role/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] dynamic request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                string role = request.role;
                if (!Enum.TryParse<UserRole>(role, true, out var userRole))
                    return BadRequest(new { message = "Invalid role" });

                user.Role = userRole;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Role assigned to user: {user.Email}, Role: {userRole}");
                return Ok(new { message = "Role assigned successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return StatusCode(500, new { message = "Error assigning role" });
            }
        }
    }
}
