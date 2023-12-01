
namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class UpdateBookVM : BaseAuditableEntityVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BookCode { get; set; }
        public double Price { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public int AuthorId { get; set; }
        public ICollection<Author>? Authors { get; set; }
        public int CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public int BrandId { get; set; }
        public ICollection<Brand>? Brands { get; set; }

        // Product Images Section 
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<BookImageVM>? BookImageVMs { get; set; }
        public List<int>? ImageIds { get; set; }
    }

    public class BookImageVM : BaseAuditableEntityVM
    {
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
    }
}
