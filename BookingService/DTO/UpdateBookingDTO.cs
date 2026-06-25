using System.ComponentModel.DataAnnotations;

namespace BookingService.DTO
{
    public class UpdateBookingDTO
    {
        public string? bookingId { get; set; }
        public string VehicleNo { get; set; }
        public string? VehicleName { get; set; }       
        public string? ServiceType { get; set; } 
        public DateOnly ServiceDate { get; set; }
    }
}
