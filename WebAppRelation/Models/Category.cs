namespace WebAppRelation.Models
{
    public class Category : BaseAuditableEntity
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<Book>? Books { get; set; }
    }
}
