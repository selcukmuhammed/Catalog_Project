using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
using MVCatalog.Services;
using Newtonsoft.Json;

namespace MVCatalog.Controllers
{
	public class CategoryController : Controller
	{
		private readonly CategoryService _category;
		public CategoryController(IConfiguration config)
		{
			_category = new CategoryService(config);
		}
		public async Task<IActionResult> Index()
		{
			var response = await _category.GetAllCategoryAsync();
			return View(response);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			CategoryViewModel model = new CategoryViewModel();
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Add(CategoryViewModel category)
		{
			var response = await _category.AddCategoryAsync(category);
			if (response)
			{
				TempData["SuccessMessage"] = "Kategori başarıyla eklendi!";
				return RedirectToAction("Index");
			}
			return RedirectToAction("Add");
		}

		[HttpGet]
		public async Task<IActionResult> Update(long id)
		{
			var response = await _category.GetCategoryByIdAsync(id);
			return View(response);
		}
		[HttpPost]
		public async Task<IActionResult> Update(CategoryViewModel category)
		{
			try
			{
				var response = await _category.UpdateCategoryAsync(category);
				if (response != null)
				{
					TempData["SuccessMessage"] = "Kategori başarıyla güncellendi!";
					return RedirectToAction("Index");
				}
				if (!response)
				{
					TempData["ErrorMessage"] = "Kategori güncellenirken bir hata oluştu.";
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
				var response = await _category.DeleteCategoryAsync(id);
				if (response != null)
				{
					TempData["SuccessMessage"] = "Kategori başarıyla silindi!";
					return RedirectToAction("Index");
				}
				if (!response)
				{
					TempData["ErrorMessage"] = "Kategori silinirken bir hata oluştu.";
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
