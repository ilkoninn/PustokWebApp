namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class CreateCategoryVM : BaseAuditableEntityVM
    {
        public string Name { get; set; }
        public string? ParentCategoryId { get; set; }
        public ICollection<Category>? categories { get; set; }
    }
}
