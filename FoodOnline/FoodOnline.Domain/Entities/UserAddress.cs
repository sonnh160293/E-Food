using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class UserAddress : BaseEntity
    {
        public string? City { get; set; } = string.Empty;
        public string? District { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
        public string? Detail { get; set; } = string.Empty;
        public bool IsDefault { get; set; }


        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; } = null!;
    }
}
