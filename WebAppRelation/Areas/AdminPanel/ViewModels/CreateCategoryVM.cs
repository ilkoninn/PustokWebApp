namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class CreateCategoryVM
    {
        public string Name { get; set; }
        public string? ParentCategoryId { get; set; }
        public ICollection<Category>? categories { get; set; }
        public List<Book>? Books { get; set; }
    }
}
