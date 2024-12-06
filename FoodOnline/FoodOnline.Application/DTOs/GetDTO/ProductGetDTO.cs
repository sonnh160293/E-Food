namespace FoodOnline.Application.DTOs.GetDTO
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Weight { get; set; }
        public int? Volume { get; set; }
        public double? UnitPrice { get; set; }
        public int? Discount { get; set; }
        public string? ImageURL { get; set; }


        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }


        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

    }
}
