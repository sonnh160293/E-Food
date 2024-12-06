using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOnline.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public long OrderCode { get; set; }
        public string? Note { get; set; } = string.Empty;
        public string? ReceiverName { get; set; } = string.Empty;
        public DateTime? OrderedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? ReceivedTime { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? ReceiverAddress { get; set; }
        public double? ShippingFee { get; set; }
        public double? TotalPrice { get; set; }

        public bool? IsCOD { get; set; }

        public bool? IsPaid { get; set; }
        public bool? IsConfirm { get; set; }
        public int? BranchId { get; set; }
        public string? CustomerId { get; set; }
        public int? StatusId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch? Branch { get; set; } = null!;

        [ForeignKey("StatusId")]
        public virtual OrderStatus? OrderStatus { get; set; } = null!;
        [ForeignKey("CustomerId")]
        public virtual ApplicationUser? Customer { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
