using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Tag : BaseAuditableEntity
    {
        public string Name { get; set; }
        public int? BookId { get; set; }
        public Book? Book { get; set; }
        public int? BlogId { get; set; }
        public Blog? Blog { get; set; }
        public ICollection<Book>? Books { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
    }
}
