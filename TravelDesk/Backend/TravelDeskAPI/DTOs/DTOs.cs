using System.ComponentModel.DataAnnotations;

namespace TravelDeskAPI.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUserRequest
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
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
        public string Role { get; set; } = string.Empty;

        public string? ManagerName { get; set; }
        public int? ManagerId { get; set; }
    }

    public class UpdateUserRequest
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string? ManagerName { get; set; }
        public int? ManagerId { get; set; }
    }

    public class TravelRequestDTO
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string ReasonForTravelling { get; set; } = string.Empty;
        public string TypeOfBooking { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public int? DaysOfStay { get; set; }
        public string? MealRequired { get; set; }
        public string? MealPreference { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }

    public class CreateTravelRequestRequest
    {
        [Required]
        [StringLength(20)]
        public string EmployeeID { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ReasonForTravelling { get; set; } = string.Empty;

        [Required]
        public string TypeOfBooking { get; set; } = string.Empty;

        public string? AadharNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? DaysOfStay { get; set; }
        public string? MealRequired { get; set; }
        public string? MealPreference { get; set; }
    }

    public class CommentDTO
    {
        public int Id { get; set; }
        public int TravelRequestId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateCommentRequest
    {
        [Required]
        [StringLength(1000)]
        public string CommentText { get; set; } = string.Empty;
    }

    public class DocumentDTO
    {
        public int Id { get; set; }
        public int TravelRequestId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileURL { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
    }
}
