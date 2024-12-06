namespace FoodOnline.Application.DTOs.GetDTO
{
    public class OrderDetailGetDTO
    {
        public int? ProductId { get; set; }

        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
        public int? Discount { get; set; }

        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? ProductImage { get; set; }

    }
}
