using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.DTO
{
    public class CreateBookingDTO
    {
        public string? CustomerName { get; set; }
        [Column(TypeName ="varchar(10)")]
        public string? VehicleName { get; set; }
        [RegularExpression(@"^[A-Z]{2}\s?\d{2}\s?[A-Z]{2}\s?\d{4}$")]
        [Column(TypeName = ("varchar(10"))]
        public string? VehicleNo { get; set; }
        public string? VehicleType { get; set; }
        public string ServiceCenterId { get; set; }
        public string? ServiceType { get; set; } 
        public DateOnly ServiceDate { get; set; }

    }
}
