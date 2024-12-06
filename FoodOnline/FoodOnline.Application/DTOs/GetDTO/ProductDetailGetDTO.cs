namespace FoodOnline.Application.DTOs.GetDTO
{
    public class ProductDetailGetDTO
    {
        public int? UnitInStock { get; set; }
        public bool? IsActive { get; set; }


        public int? ProductId { get; set; }
        public string? ProductName { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? CategoryName { get; set; }

        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
