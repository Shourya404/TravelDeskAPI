using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelDeskAPI.Data;
using TravelDeskAPI.DTOs;
using TravelDeskAPI.Models;

namespace TravelDeskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TravelRequestsController : ControllerBase
    {
        private readonly TravelDeskDbContext _context;
        private readonly ILogger<TravelRequestsController> _logger;

        public TravelRequestsController(TravelDeskDbContext context, ILogger<TravelRequestsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private string GenerateRequestNumber()
        {
            return $"TR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        [HttpPost("create")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateTravelRequest([FromBody] CreateTravelRequestRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request" });

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var employeeId))
                    return Unauthorized(new { message = "Invalid user" });

                var employee = await _context.Users.FindAsync(employeeId);
                if (employee == null || employee.Role != UserRole.Employee)
                    return Unauthorized(new { message = "Only employees can create travel requests" });

                if (!Enum.TryParse<BookingType>(request.TypeOfBooking, true, out var bookingType))
                    return BadRequest(new { message = "Invalid booking type" });

                var travelRequest = new TravelRequest
                {
                    RequestNumber = GenerateRequestNumber(),
                    EmployeeId = employeeId,
                    EmployeeID = request.EmployeeID,
                    EmployeeName = request.EmployeeName,
                    ProjectName = request.ProjectName,
                    DepartmentName = request.DepartmentName,
                    ReasonForTravelling = request.ReasonForTravelling,
                    TypeOfBooking = bookingType,
                    AadharNumber = request.AadharNumber,
                    PassportNumber = request.PassportNumber,
                    TravelDate = request.TravelDate,
                    DaysOfStay = request.DaysOfStay,
                    Status = RequestStatus.Draft,
                    CreatedDate = DateTime.UtcNow
                };

                if (!string.IsNullOrEmpty(request.MealRequired) && Enum.TryParse<MealType>(request.MealRequired, true, out var mealType))
                    travelRequest.MealRequired = mealType;

                if (!string.IsNullOrEmpty(request.MealPreference) && Enum.TryParse<MealPreference>(request.MealPreference, true, out var mealPref))
                    travelRequest.MealPreference = mealPref;

                _context.TravelRequests.Add(travelRequest);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Travel request created: {travelRequest.RequestNumber}");
                return CreatedAtAction(nameof(GetTravelRequestById), new { id = travelRequest.Id }, 
                    new { message = "Travel request created successfully", requestId = travelRequest.Id, requestNumber = travelRequest.RequestNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating travel request");
                return StatusCode(500, new { message = "Error creating travel request" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTravelRequestById(int id)
        {
            try
            {
                var request = await _context.TravelRequests
                    .Include(tr => tr.Documents)
                    .Include(tr => tr.Comments)
                    .ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(tr => tr.Id == id && !tr.IsDeleted);

                if (request == null)
                    return NotFound(new { message = "Travel request not found" });

                var dto = new TravelRequestDTO
                {
                    Id = request.Id,
                    RequestNumber = request.RequestNumber,
                    EmployeeID = request.EmployeeID,
                    EmployeeName = request.EmployeeName,
                    ProjectName = request.ProjectName,
                    DepartmentName = request.DepartmentName,
                    ReasonForTravelling = request.ReasonForTravelling,
                    TypeOfBooking = request.TypeOfBooking.ToString(),
                    Status = request.Status.ToString(),
                    TravelDate = request.TravelDate ?? DateTime.MinValue,
                    DaysOfStay = request.DaysOfStay,
                    MealRequired = request.MealRequired?.ToString(),
                    MealPreference = request.MealPreference?.ToString(),
                    CreatedDate = request.CreatedDate,
                    SubmittedDate = request.SubmittedDate
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting travel request");
                return StatusCode(500, new { message = "Error retrieving travel request" });
            }
        }

        [HttpPost("{id}/submit")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> SubmitTravelRequest(int id)
        {
            try
            {
                var travelRequest = await _context.TravelRequests
                    .Include(tr => tr.Employee)
                    .FirstOrDefaultAsync(tr => tr.Id == id && !tr.IsDeleted);

                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                if (travelRequest.Status != RequestStatus.Draft && travelRequest.Status != RequestStatus.ReturnedToEmployee)
                    return BadRequest(new { message = "Only draft or returned requests can be submitted" });

                // Validate required fields
                if (string.IsNullOrEmpty(travelRequest.EmployeeID) || string.IsNullOrEmpty(travelRequest.ProjectName) ||
                    travelRequest.TravelDate == null)
                    return BadRequest(new { message = "All questions are mandatory to fill by the user" });

                travelRequest.Status = RequestStatus.SubmittedToManager;
                travelRequest.SubmittedDate = DateTime.UtcNow;

                _context.TravelRequests.Update(travelRequest);
                await _context.SaveChangesAsync();

                // TODO: Send email notification to manager
                _logger.LogInformation($"Travel request submitted: {travelRequest.RequestNumber}");

                return Ok(new { message = "Travel request submitted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting travel request");
                return StatusCode(500, new { message = "Error submitting travel request" });
            }
        }

        [HttpPost("{id}/delete")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteTravelRequest(int id)
        {
            try
            {
                var travelRequest = await _context.TravelRequests.FindAsync(id);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                if (travelRequest.Status != RequestStatus.Draft)
                    return BadRequest(new { message = "Only draft requests can be deleted" });

                travelRequest.IsDeleted = true;
                travelRequest.DeletedDate = DateTime.UtcNow;

                _context.TravelRequests.Update(travelRequest);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Travel request deleted: {travelRequest.RequestNumber}");
                return Ok(new { message = "Travel request deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting travel request");
                return StatusCode(500, new { message = "Error deleting travel request" });
            }
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveTravelRequest(int id, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.CommentText))
                return BadRequest(new { message = "Comments cannot be left blank" });

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var managerId))
                    return Unauthorized(new { message = "Invalid user" });

                var travelRequest = await _context.TravelRequests.FindAsync(id);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                if (travelRequest.Status != RequestStatus.SubmittedToManager)
                    return BadRequest(new { message = "Request must be in SubmittedToManager status" });

                travelRequest.Status = RequestStatus.ApprovedByManager;
                travelRequest.ManagerId = managerId;
                travelRequest.ModifiedDate = DateTime.UtcNow;

                var comment = new Comment
                {
                    TravelRequestId = id,
                    UserId = managerId,
                    CommentText = request.CommentText,
                    CreatedDate = DateTime.UtcNow
                };

                _context.TravelRequests.Update(travelRequest);
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // TODO: Send notification to HR Travel Admin
                _logger.LogInformation($"Travel request approved: {travelRequest.RequestNumber}");

                return Ok(new { message = "Travel request approved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving travel request");
                return StatusCode(500, new { message = "Error approving travel request" });
            }
        }

        [HttpPost("{id}/disapprove")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DisapproveTravelRequest(int id, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.CommentText))
                return BadRequest(new { message = "Comments cannot be left blank" });

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var managerId))
                    return Unauthorized(new { message = "Invalid user" });

                var travelRequest = await _context.TravelRequests.FindAsync(id);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                travelRequest.Status = RequestStatus.RejectedByManager;
                travelRequest.ManagerId = managerId;
                travelRequest.ModifiedDate = DateTime.UtcNow;

                var comment = new Comment
                {
                    TravelRequestId = id,
                    UserId = managerId,
                    CommentText = request.CommentText,
                    CreatedDate = DateTime.UtcNow
                };

                _context.TravelRequests.Update(travelRequest);
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // TODO: Send notification to employee
                _logger.LogInformation($"Travel request rejected: {travelRequest.RequestNumber}");

                return Ok(new { message = "Travel request disapproved successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disapproving travel request");
                return StatusCode(500, new { message = "Error disapproving travel request" });
            }
        }

        [HttpPost("{id}/return-to-employee")]
        [Authorize(Roles = "Manager,HRTravelAdmin")]
        public async Task<IActionResult> ReturnToEmployee(int id, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.CommentText))
                return BadRequest(new { message = "Comments cannot be left blank" });

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var userId))
                    return Unauthorized(new { message = "Invalid user" });

                var travelRequest = await _context.TravelRequests.FindAsync(id);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                travelRequest.Status = RequestStatus.ReturnedToEmployee;
                travelRequest.ModifiedDate = DateTime.UtcNow;

                var comment = new Comment
                {
                    TravelRequestId = id,
                    UserId = userId,
                    CommentText = request.CommentText,
                    CreatedDate = DateTime.UtcNow
                };

                _context.TravelRequests.Update(travelRequest);
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // TODO: Send notification to employee
                _logger.LogInformation($"Travel request returned to employee: {travelRequest.RequestNumber}");

                return Ok(new { message = "Travel request returned to employee successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning travel request");
                return StatusCode(500, new { message = "Error returning travel request" });
            }
        }
    }
}
