using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Category : BaseAuditableEntity
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public IEnumerable<Book>? Books { get; set; }
    }
}
