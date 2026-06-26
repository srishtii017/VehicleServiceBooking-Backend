using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Model
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public class Booking
    {
        [Required]
        public string BookingId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? CustomerName { get; set;  }
        public string VehicleName { get; set; }
        [Required]
        public string? VehicleNo {get;set;}
        [Required]
        public string ServiceCenterId { get; set; }
        [Required]
        public string Phone_No {get; set; }
        public string? Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateOnly ServiceDate { get; set; }
        public string Status { get; set; } = BookingStatus.Pending.ToString();

    }
}
