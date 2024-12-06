namespace FoodOnline.Application.DTOs
{
    public class BranchSearchRequest : BaseSearchRequest
    {
        public string? Keyword { get; set; }
        public bool? Status { get; set; }
    }

    public class CategorySearchRequest : BaseSearchRequest
    {
        public int? CategoryId { get; set; }
    }

    public class OrderSeachRequest : BaseSearchRequest
    {
        public int? StatusId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class OrderSeachRequestAdmin : BaseSearchRequest
    {
        public int? StatusId { get; set; }

        public bool? IsPaid { get; set; }
        public bool? IsConfirm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
