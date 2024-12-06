namespace FoodOnline.Domain.Entities
{
    public interface IAuditedEntityBase
    {
        string? CreatedBy { get; set; }

        DateTime? CreatedDate { get; set; }

        string? LastModifiedBy { get; set; }

        DateTime? LastModifiedDate { get; set; }
        bool? IsDeleted { get; set; }
        string? DeletedBy { get; set; }
    }
}
