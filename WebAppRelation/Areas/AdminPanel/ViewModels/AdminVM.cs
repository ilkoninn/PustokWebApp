namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class AdminVM
    {
        public ICollection<Book> Books { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Brand> Brands { get; set; }
        public ICollection<BookImages> BookImages { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
