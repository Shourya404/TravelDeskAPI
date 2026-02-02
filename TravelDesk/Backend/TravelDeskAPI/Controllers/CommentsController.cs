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
    public class CommentsController : ControllerBase
    {
        private readonly TravelDeskDbContext _context;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(TravelDeskDbContext context, ILogger<CommentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("{travelRequestId}")]
        public async Task<IActionResult> AddComment(int travelRequestId, [FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(request.CommentText))
                return BadRequest(new { message = "Comment text cannot be empty" });

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var userId))
                    return Unauthorized(new { message = "Invalid user" });

                var travelRequest = await _context.TravelRequests.FindAsync(travelRequestId);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                var comment = new Comment
                {
                    TravelRequestId = travelRequestId,
                    UserId = userId,
                    CommentText = request.CommentText,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Comment added to request: {travelRequest.RequestNumber}");
                return CreatedAtAction(nameof(GetCommentsByTravelRequest), new { travelRequestId }, 
                    new { message = "Comment added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment");
                return StatusCode(500, new { message = "Error adding comment" });
            }
        }

        [HttpGet("{travelRequestId}")]
        public async Task<IActionResult> GetCommentsByTravelRequest(int travelRequestId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(c => c.TravelRequestId == travelRequestId && !c.IsDeleted)
                    .Include(c => c.User)
                    .OrderByDescending(c => c.CreatedDate)
                    .Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        TravelRequestId = c.TravelRequestId,
                        UserName = $"{c.User.FirstName} {c.User.LastName}",
                        CommentText = c.CommentText,
                        CreatedDate = c.CreatedDate
                    })
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting comments");
                return StatusCode(500, new { message = "Error retrieving comments" });
            }
        }
    }
}
