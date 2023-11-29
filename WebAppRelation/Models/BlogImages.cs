using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRelation.Models
{
    public class BlogImages : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
