using System.ComponentModel.DataAnnotations;

namespace SchoolSystem0.Server.Model
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Example: "Admin", "Teacher", etc.
    }

    // Model for login
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
