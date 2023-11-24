 namespace WebAppRelation.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Tag> Tag { get; set; }
    }
}
