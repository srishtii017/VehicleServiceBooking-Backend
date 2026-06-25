using System.ComponentModel.DataAnnotations;

namespace BookingService.Model
{
    public class User
    {
        public int? UserID { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? PasswordHash { get; set; }
    }
}
