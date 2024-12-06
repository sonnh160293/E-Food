using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class CategoryPostDTO
    {
        [AllowNull]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name is too long")]
        public string? Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }
}
