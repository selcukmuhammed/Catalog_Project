﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCatalog.Models
{
    public class ProductViewModel
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecordTime { get; set; }
        public long CategoryId { get; set; }
        public CategoryViewModel Categories { get; set; }
		public List<CategoryViewModel> CategoryList { get; set; }
	}
}
