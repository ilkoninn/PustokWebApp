namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class BaseAuditableEntityVM : BaseEntityVM
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set;}
    }
}
