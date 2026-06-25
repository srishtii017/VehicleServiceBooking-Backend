using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.DTO
{
    public class CreateBookingDTO
    {
        [Column(TypeName ="varchar(10)")]
        public string? VehicleName { get; set; }
        public string? VehicleNo { get; set; }
        public string ServiceCenterId { get; set; }
        public string? ServiceType { get; set; } 
        public DateOnly ServiceDate { get; set; }

    }
}
