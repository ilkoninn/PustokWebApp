

using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRelation.Models
{
    public class Blog : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<BlogTag> Tags { get; set; }
    }
}
