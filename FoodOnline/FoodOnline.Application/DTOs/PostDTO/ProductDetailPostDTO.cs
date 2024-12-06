using System.ComponentModel.DataAnnotations;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class ProductDetailPostDTO
    {
        [Required(ErrorMessage = "Discount is required")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Discount must be greater than 0 and < 100")]
        public int? UnitInStock { get; set; }
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Discount is required")]
        public int? BranchId { get; set; }



    }
}
