 namespace WebAppRelation.Models
{
    public class Blog : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public List<Tag> Tag { get; set; }
    }
}
