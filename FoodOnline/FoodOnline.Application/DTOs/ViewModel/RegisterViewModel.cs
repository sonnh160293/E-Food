using System.ComponentModel.DataAnnotations;

namespace FoodOnline.Application.DTOs.ViewModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(200, ErrorMessage = "City is too long")]
        public string? City { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        [StringLength(200, ErrorMessage = "District is too long")]
        public string? District { get; set; } = string.Empty;



        [Required(ErrorMessage = "Ward is required")]
        [StringLength(200, ErrorMessage = "Ward is too long")]
        public string? Ward { get; set; } = string.Empty;



        [Required(ErrorMessage = "Detail is required")]
        [StringLength(200, ErrorMessage = "Detail is too long")]
        public string? Detail { get; set; } = string.Empty;
    }
}
