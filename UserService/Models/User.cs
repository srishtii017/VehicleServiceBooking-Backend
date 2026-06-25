using System.ComponentModel.DataAnnotations;

namespace User_Management.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "Phone must be between 10 and 13 characters.")]
        public string Phone { get; set; }


        [Required]
        public string FlatNumber { get; set; }

        [Required]
        public string Street { get; set; }

        public string? Landmark { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Pincode { get; set; }


        [Required]
        public string PasswordHash { get; set; }
    }
}
