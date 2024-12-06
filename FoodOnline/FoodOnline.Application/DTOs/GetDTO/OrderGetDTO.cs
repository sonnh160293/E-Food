namespace FoodOnline.Application.DTOs.GetDTO
{
    public class OrderGetDTO
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
        public bool? IsPaid { get; set; }
        public bool? IsConfirm { get; set; }

        public bool? IsCOD { get; set; }
        public int? BranchId { get; set; }
        public string? CustomerId { get; set; }
        public int? StatusId { get; set; }
        public string? BranchName { get; set; }
        public string? Status { get; set; }
        public string? UserOrder { get; set; }


    }
}
