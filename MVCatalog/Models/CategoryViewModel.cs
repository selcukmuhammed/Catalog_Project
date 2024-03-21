using System.ComponentModel.DataAnnotations;

namespace MVCatalog.Models
{
    public class CategoryViewModel
    {
        [Key]
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecordTime { get; set; }
    }
}
