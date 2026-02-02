using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelDeskAPI.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TravelRequest")]
        public int TravelRequestId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("TravelRequestId")]
        public virtual TravelRequest TravelRequest { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
