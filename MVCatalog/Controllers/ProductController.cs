using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCatalog.Models;
using MVCatalog.Services;
using Newtonsoft.Json;

namespace MVCatalog.Controllers
{
	public class ProductController : Controller
	{
		private readonly ProductService _product;
		private readonly CategoryService _category;

		public ProductController(IConfiguration config)
		{
			_product = new ProductService(config);
			_category = new CategoryService(config);
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _product.GetAllProductsAsync();
			return View(response);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			ProductViewModel product = new ProductViewModel();
			var category = await _category.GetAllCategoryAsync();

			var models = (product, category);
			return View(models);
		}

		[HttpPost]
		public async Task<IActionResult> Add( ProductViewModel product)
		{
			var response = await _product.AddProductAsync(product);
			if (response)
			{
				TempData["SuccessMessage"] = "Ürün başarıyla eklendi!";
				return RedirectToAction("Index");
			}
			return RedirectToAction("Add");
		}

		[HttpGet]
		public async Task<IActionResult> Update(long id)
		{
			var response = await _product.GetProductByIdAsync(id);
			var categories = await _category.GetAllCategoryAsync();

			response.CategoryList = categories;

			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Update(ProductViewModel product)
		{
			try
			{
				var response = await _product.UpdateProductAsync(product);
				if (response != null)
				{
					TempData["SuccessMessage"] = "Ürün başarıyla güncellendi!";
					return RedirectToAction("Index");
				}
				if (!response)
				{
					TempData["ErrorMessage"] = "Ürün güncellenirken bir hata oluştu.";
					return RedirectToAction();
				}
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Delete(long id)
		{
			try
			{
				var response = await _product.DeleteProductAsync(id);
				if (response != null)
				{
					TempData["SuccessMessage"] = "Ürün başarıyla silindi!";
					return RedirectToAction("Index");
				}
				if (!response)
				{
					TempData["ErrorMessage"] = "Ürün silinirken bir hata oluştu.";
					return RedirectToAction();
				}
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
