using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRelation.Models
{
    public class Author : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Book>? Books { get; set; }
        [NotMapped]
        
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", Name, Surname);
            }
        }
    }
}
