using System.ComponentModel.DataAnnotations;

namespace BookingService.Model
{
    public class User
    {
        public int? UserID { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
        public string FlatNumber { get; set; }
        public string Street { get; set; }
        public string? Landmark { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string Pincode { get; set; }

    }
}
