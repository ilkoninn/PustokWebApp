using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Brand : BaseAuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Book>? Books { get; set; }
    }
}
