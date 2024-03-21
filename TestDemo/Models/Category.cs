using System.ComponentModel.DataAnnotations;

namespace TestDemo.Models
{
    public class Category
    {
        [Key]
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecordTime { get; set; }
    }
}
