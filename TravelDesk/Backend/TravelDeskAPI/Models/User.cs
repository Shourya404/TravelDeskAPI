using System.ComponentModel.DataAnnotations;

namespace TravelDeskAPI.Models
{
    public enum UserRole
    {
        Admin = 1,
        HRTravelAdmin = 2,
        Employee = 3,
        Manager = 4
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string EmployeeID { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [StringLength(50)]
        public string? ManagerName { get; set; }

        public int? ManagerId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public virtual ICollection<TravelRequest> TravelRequests { get; set; } = new List<TravelRequest>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
