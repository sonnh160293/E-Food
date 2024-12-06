namespace FoodOnline.Domain.Entities
{
    public abstract class AuditedBaseEntity : IAuditedEntityBase
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
    }
}
