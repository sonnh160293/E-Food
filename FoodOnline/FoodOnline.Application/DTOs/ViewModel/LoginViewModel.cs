using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.ViewModel
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Username is required")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]

        public string Password { get; set; }
        [AllowNull]
        public string ReturnUrl { get; set; } = String.Empty;

        public bool HasRemember { get; set; }
    }
}
