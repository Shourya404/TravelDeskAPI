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
    public class DocumentsController : ControllerBase
    {
        private readonly TravelDeskDbContext _context;
        private readonly ILogger<DocumentsController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DocumentsController(TravelDeskDbContext context, ILogger<DocumentsController> logger, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost("upload/{travelRequestId}")]
        public async Task<IActionResult> UploadDocument(int travelRequestId, [FromForm] IFormFile file, [FromForm] string documentType)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            try
            {
                var travelRequest = await _context.TravelRequests.FindAsync(travelRequestId);
                if (travelRequest == null)
                    return NotFound(new { message = "Travel request not found" });

                if (!Enum.TryParse<DocumentType>(documentType, true, out var docType))
                    return BadRequest(new { message = "Invalid document type" });

                // Save file to uploads folder
                var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{travelRequestId}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var document = new Document
                {
                    TravelRequestId = travelRequestId,
                    FileName = file.FileName,
                    FileURL = $"/uploads/{fileName}",
                    DocumentType = docType,
                    UploadedDate = DateTime.UtcNow
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Document uploaded: {fileName}");
                return CreatedAtAction(nameof(GetDocumentsByTravelRequest), 
                    new { travelRequestId }, 
                    new { message = "Document uploaded successfully", documentId = document.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                return StatusCode(500, new { message = "Error uploading document" });
            }
        }

        [HttpGet("{travelRequestId}")]
        public async Task<IActionResult> GetDocumentsByTravelRequest(int travelRequestId)
        {
            try
            {
                var documents = await _context.Documents
                    .Where(d => d.TravelRequestId == travelRequestId && !d.IsDeleted)
                    .OrderByDescending(d => d.UploadedDate)
                    .Select(d => new DocumentDTO
                    {
                        Id = d.Id,
                        TravelRequestId = d.TravelRequestId,
                        FileName = d.FileName,
                        FileURL = d.FileURL,
                        DocumentType = d.DocumentType.ToString(),
                        UploadedDate = d.UploadedDate
                    })
                    .ToListAsync();

                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting documents");
                return StatusCode(500, new { message = "Error retrieving documents" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var document = await _context.Documents.FindAsync(id);
                if (document == null)
                    return NotFound(new { message = "Document not found" });

                document.IsDeleted = true;
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Document deleted: {document.FileName}");
                return Ok(new { message = "Document deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document");
                return StatusCode(500, new { message = "Error deleting document" });
            }
        }
    }
}
