using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class UserAddressPostDTO
    {
        [AllowNull]
        public int Id { get; set; }
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
        public bool IsDefault { get; set; }


        public string? UserId { get; set; }
    }
}
