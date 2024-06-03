using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCatalog.Models
{
    public class ProductViewModel
    {
        public long ProductId { get; set; }

		[Required(ErrorMessage = "Ürün adı zorunludur.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Ürün fiyatı zorunludur.")]
		public decimal Price { get; set; }
        public bool IsDeleted { get; set; }

		[Required(ErrorMessage = "Ürün tarihi zorunludur.")]
		public DateTime RecordTime { get; set; }

		[Required(ErrorMessage = "Kategori seçiniz.")]
		public long CategoryId { get; set; }
        public CategoryViewModel Categories { get; set; }
		public List<CategoryViewModel> CategoryList { get; set; }
	}
}
