namespace FoodOnline.Application.DTOs.GetDTO
{
    public class UserGetDTO
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IsActive { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }

    }
}
