using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Book : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BookCode { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public Brand? Brand { get; set; }
        public Category? Category { get; set; }
        public List<BookImages>? BookImages { get; set; }
        public ICollection<BookTag>? Tags { get; set; }
    }
}
