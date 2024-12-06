using System.Diagnostics.CodeAnalysis;

namespace FoodOnline.Application.DTOs.PostDTO
{
    public class OrderPostDTO
    {
        public int Id { get; set; }
        public long OrderCode { get; set; }
        [AllowNull]
        public string? Note { get; set; } = string.Empty;
        public string? ReceiverName { get; set; } = string.Empty;
        [AllowNull]
        public DateTime? OrderedDate { get; set; }
        [AllowNull]
        public DateTime? ReceivedDate { get; set; }
        [AllowNull]
        public string? ReceivedTime { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? ReceiverAddress { get; set; }
        public double? ShippingFee { get; set; }
        public double? TotalPrice { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsCOD { get; set; }
        public bool? IsConfirm { get; set; }
        [AllowNull]
        public int? BranchId { get; set; }
        [AllowNull]
        public string? CustomerId { get; set; }
        [AllowNull]
        public int? StatusId { get; set; }
    }
}
