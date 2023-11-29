namespace WebAppRelation.Models
{
    public class BookTag : BaseAuditableEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
