using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRelation.Models
{
    public class BookImages : BaseAuditableEntity
    {
        public string? ImgUrl { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
