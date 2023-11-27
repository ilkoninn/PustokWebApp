namespace WebAppRelation.Models
{
    public class Tag : BaseAuditableEntity
    {
        public string Name { get; set; }
        public List<Book> Book { get; set; }
        public List<Blog> Blog { get; set; }
    }
}
