using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int? ProductId { get; set; }

        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? UnitPrice { get; set; }
        public int? Discount { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; } = null!;

        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; } = null!;

    }
}
