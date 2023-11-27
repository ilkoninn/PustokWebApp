namespace WebAppRelation.Models
{
    public class Brand : BaseAuditableEntity
    {
        public string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
