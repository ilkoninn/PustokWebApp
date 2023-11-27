namespace WebAppRelation.Models
{
    public class BaseAuditableEntity:BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
    }
}
