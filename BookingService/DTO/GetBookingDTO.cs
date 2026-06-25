using BookingService.Model;

namespace BookingService.DTO
{
    public class GetBookingDTO
    {
        public string? BookingId { get; set; }
        public string? VehicleName { get; set; }
        public string? VehicleNo { get; set; }
        public string ServiceCenterId { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public DateOnly ServiceDate { get; set; }
        public string? Status { get; set; } 
        public DateTime CreatedDate { get; set; }
    }
}
