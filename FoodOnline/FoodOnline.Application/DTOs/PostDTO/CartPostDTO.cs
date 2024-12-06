using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class CartPostDTO
    {
        [AllowNull]
        public int Id { get; set; }
        public string CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
