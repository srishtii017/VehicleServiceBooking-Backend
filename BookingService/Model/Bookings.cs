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
        public string? BookingId { get; set; }
        public int UserId { get; set; }
        public string? CustomerName { get; set;  }
        public string? VehicleName { get; set; }
        public string? VehicleNo {get;set;}
        public string ServiceCenterId { get; set; }
        public string? Phone_No {get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateOnly ServiceDate { get; set; }
        public string Status { get; set; } = BookingStatus.Pending.ToString();

    }
}
