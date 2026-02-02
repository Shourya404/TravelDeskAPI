using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelDeskAPI.Models
{
    public enum DocumentType
    {
        AadharCard = 1,
        Passport = 2,
        Visa = 3,
        Ticket = 4,
        HotelConfirmation = 5,
        Other = 6
    }

    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TravelRequest")]
        public int TravelRequestId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FileURL { get; set; } = string.Empty;

        [Required]
        public DocumentType DocumentType { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        // Navigation property
        [ForeignKey("TravelRequestId")]
        public virtual TravelRequest TravelRequest { get; set; } = null!;
    }
}
