namespace WebAppRelation.Models
{
    public class BasketItem : BaseAuditableEntity
    {
        public int Count { get; set; }
        public double Price { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
