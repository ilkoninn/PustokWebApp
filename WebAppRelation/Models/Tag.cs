using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Tag : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<BookTag>? Books { get; set; }
        public ICollection<BlogTag>? Blogs { get; set; }
    }
}
