using Microsoft.AspNetCore.Mvc;
using TravelDeskAPI.DTOs;
using TravelDeskAPI.Services;

namespace TravelDeskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthenticationService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var (success, message, data) = await _authService.LoginAsync(request.Email, request.Password);

            if (!success)
            {
                _logger.LogWarning($"Failed login attempt for email: {request.Email}");
                return Unauthorized(new { message });
            }

            _logger.LogInformation($"Successful login for email: {request.Email}");
            return Ok(new { message, data });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            var (success, message) = await _authService.RegisterAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.EmployeeID,
                request.Department,
                request.Role
            );

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }
    }
}
