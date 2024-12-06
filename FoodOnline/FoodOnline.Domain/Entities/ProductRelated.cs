using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class ProductRelated : BaseEntity
    {

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int ProductRelatedId { get; set; }
        [ForeignKey("ProductRelatedId")]
        public virtual Product RelatedProduct { get; set; }

    }
}
