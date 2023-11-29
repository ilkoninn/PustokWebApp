using System.ComponentModel.DataAnnotations.Schema;
using WebAppRelation.Models.Entity;

namespace WebAppRelation.Models
{
    public class Author : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public IEnumerable<Book> Books { get; set; }
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
