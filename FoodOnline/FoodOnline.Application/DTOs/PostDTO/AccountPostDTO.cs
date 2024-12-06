using System.ComponentModel.DataAnnotations;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class AccountPostDTO
    {

        public string RoleName { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public int? BranchId { get; set; }
        public bool IsActive { get; set; }
    }
}
