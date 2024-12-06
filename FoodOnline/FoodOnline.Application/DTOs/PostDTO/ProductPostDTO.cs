using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class ProductPostDTO
    {
        [AllowNull]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name is too long")]
        public string? Name { get; set; }

        [AllowNull]
        public string? Description { get; set; }

        [AllowNull]
        public int? Weight { get; set; }
        [AllowNull]
        public int? Volume { get; set; }
        [Required(ErrorMessage = "Unit Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than 0")]
        public double? UnitPrice { get; set; }
        [Required(ErrorMessage = "Discount is required")]
        [Range(0, 100, ErrorMessage = "Discount must be greater than 0 and < 100")]
        public int? Discount { get; set; }

        [AllowNull]
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }

        [AllowNull]
        public IFormFile Image { get; set; }
    }
}
