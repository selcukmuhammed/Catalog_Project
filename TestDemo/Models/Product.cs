using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDemo.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecordTime { get; set; }

        //FOREIGN KEY
        
        public long CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public required Category Categories { get; set; }
	}
}
