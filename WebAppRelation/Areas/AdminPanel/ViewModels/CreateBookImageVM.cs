using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Areas.AdminPanel.ViewModels.Entity;

namespace WebAppRelation.Areas.AdminPanel.ViewModels
{
    public class CreateBookImageVM : BaseAuditableEntityVM
    {
        public string? ImgUrl { get; set; }
        public string BookId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
