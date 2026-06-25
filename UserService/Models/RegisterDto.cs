using System.ComponentModel.DataAnnotations;

namespace User_Management.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(13, MinimumLength = 10)]
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



        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}