using WebAppRelation.Areas.AdminPanel.ViewModels.Entity;

namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class CreateBookVM : BaseAuditableEntityVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BookCode { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public int AuthorId { get; set; }
        public ICollection<Author>? Authors { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set;}
        public int BrandId { get; set; }
        public ICollection<Brand>? Brands { get; set; }
    }
}
