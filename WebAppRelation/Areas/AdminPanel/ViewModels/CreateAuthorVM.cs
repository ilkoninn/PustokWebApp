using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Models.Entity;

namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class CreateAuthorVM : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [NotMapped]
        public string? FullName
        {
            get
            {
                return string.Format("{0} {1}", Name, Surname);
            }
        }
    }
}
