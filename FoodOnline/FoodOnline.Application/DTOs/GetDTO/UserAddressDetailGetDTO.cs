namespace FoodOnline.Application.DTOs.GetDTO
{
    public class UserAddressDetailGetDTO
    {
        public int Id { get; set; }
        public string? City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
        public string? Detail { get; set; } = string.Empty;
        public bool IsDefault { get; set; }


        public string? UserId { get; set; }
    }
}
