using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class ProductDetail : BaseEntity
    {


        public int? UnitInStock { get; set; }
        public bool? IsActive { get; set; }


        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; } = null!;
        public int? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch? Branch { get; set; } = null!;

    }
}
