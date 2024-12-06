namespace FoodOnline.Application.DTOs.GetDTO
{
    public class ProductCartDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? Weight { get; set; }
        public int? Volume { get; set; }
        public double? UnitPrice { get; set; }

        public string? ImageURL { get; set; }
        public int Quantity { get; set; }
        public double? Total { get; set; }
        public string? CategoryName { get; set; }
    }
}
