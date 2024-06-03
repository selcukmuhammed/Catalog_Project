using System.ComponentModel.DataAnnotations;

namespace MVCatalog.Models
{
    public class CategoryViewModel
    {
        [Key]
        public long CategoryId { get; set; }

		[Required(ErrorMessage = "Kategori adı zorunludur.")]
		public string Name { get; set; }
        public bool IsDeleted { get; set; }

		[Required(ErrorMessage = "Kategori tarihi zorunludur.")]
		public DateTime RecordTime { get; set; }
    }
}
