namespace FoodOnline.Application.DTOs.GetDTO
{
    public class BranchDetailGetDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Ward { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
        public string? Detail { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
    }
}
