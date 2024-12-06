namespace FoodOnline.Application.DTOs.PostDTO
{
    public class OrderDetailPostDTO
    {
        public int? ProductId { get; set; }

        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
        public int? Discount { get; set; }
    }
}
