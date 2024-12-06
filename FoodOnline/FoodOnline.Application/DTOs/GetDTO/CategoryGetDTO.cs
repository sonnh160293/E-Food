namespace FoodOnline.Application.DTOs.GetDTO
{
    public class CategoryGetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        public string? IsActive { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
