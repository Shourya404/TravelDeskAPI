using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelDeskAPI.Models
{
    public enum BookingType
    {
        DomesticFlight = 1,
        InternationalFlight = 2,
        Hotel = 3,
        FlightAndHotel = 4
    }

    public enum MealType
    {
        Lunch = 1,
        Dinner = 2,
        Both = 3
    }

    public enum MealPreference
    {
        Veg = 1,
        NonVeg = 2
    }

    public enum RequestStatus
    {
        Draft = 1,
        SubmittedToManager = 2,
        ApprovedByManager = 3,
        RejectedByManager = 4,
        SubmittedToTravelAdmin = 5,
        BookingInProgress = 6,
        BookingCompleted = 7,
        ReturnedToEmployee = 8,
        ReturnedToManager = 9,
        Closed = 10
    }

    public class TravelRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RequestNumber { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

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
        public BookingType TypeOfBooking { get; set; }

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Draft;

        public bool? IsDomesticFlight { get; set; }
        [StringLength(20)]
        public string? AadharNumber { get; set; }
        [StringLength(30)]
        public string? PassportNumber { get; set; }

        public DateTime? TravelDate { get; set; }
        public int? DaysOfStay { get; set; }
        public MealType? MealRequired { get; set; }
        public MealPreference? MealPreference { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("EmployeeId")]
        public virtual User Employee { get; set; } = null!;

        [ForeignKey("ManagerId")]
        public virtual User? Manager { get; set; }

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
