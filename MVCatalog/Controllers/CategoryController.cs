using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
using MVCatalog.Services;
using Newtonsoft.Json;

namespace MVCatalog.Controllers
{
	public class CategoryController : Controller
	{
		private readonly CategoryService _category;
		private readonly IHttpContextAccessor _httpContext;
		public CategoryController(IConfiguration config, IHttpContextAccessor httpContext)
		{
			_category = new CategoryService(config, httpContext);
			_httpContext = httpContext;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var token = _httpContext.HttpContext.Session.GetString("token");
			var response = await _category.GetAllCategoryAsync();

			if (response == null)
			{
				TempData["ErrorMessage"] = "Ürünler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Login");
			}

			if (response.StatusCode == "401")
			{
				TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Index", "Login");
			}

			if (response.StatusCode == "200")
			{
				TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Index", "Category");
			}
			if (response.StatusCode == "403")
			{
				TempData["ErrorMessage"] = "Hata kodu:403. Bu alana giriş yetkiniz yoktur.";
				return RedirectToAction("Index", "Login");
			}

			if (response.Result == null)
			{
				TempData["ErrorMessage"] = "Ürünler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Category");
			}

			return View(response.Result);
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
			var token = _httpContext.HttpContext.Session.GetString("token");
			var response = await _category.AddCategoryAsync(category);

			if (response == null)
			{
				TempData["ErrorMessage"] = "Kategoriler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Login");
			}

			if (response.StatusCode == "401")
			{
				TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Index", "Login");
			}

			if (response.StatusCode == "200")
			{
				TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
				return RedirectToAction("Index", "Category");
			}
			if (response.StatusCode == "403")
			{
				TempData["ErrorMessage"] = "Hata kodu:403. Bu alana giriş yetkiniz yoktur.";
				return RedirectToAction("Index", "Login");
			}

			if (response.Result == null)
			{
				TempData["ErrorMessage"] = "Kategoriler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Category");
			}

			if (response.Result)
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
				var token = _httpContext.HttpContext.Session.GetString("token");
				var response = await _category.UpdateCategoryAsync(category);

				if (response.StatusCode == "401")
				{
					TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "200")
				{
					TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "403")
				{
					TempData["ErrorMessage"] = "Hata kodu: 403. Bu alana giriş yetkiniz yoktur.";
					return RedirectToAction("Index", "Login");
				}

				if (!response.Result)
				{
					TempData["ErrorMessage"] = "Ürün güncellenirken bir hata oluştu.";
					return RedirectToAction("Index", "Category");
				}

				TempData["SuccessMessage"] = "Ürün başarıyla güncellendi!";
				return RedirectToAction("Index", "Category");
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
				var token = _httpContext.HttpContext.Session.GetString("token");
				var response = await _category.DeleteCategoryAsync(id);

				if (response.StatusCode == "401")
				{
					TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "200")
				{
					TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "403")
				{
					TempData["ErrorMessage"] = "Hata kodu: 403. Bu alana giriş yetkiniz yoktur.";
					return RedirectToAction("Index", "Login");
				}

				if (!response.Result)
				{
					TempData["ErrorMessage"] = "Ürün silinirken bir hata oluştu.";
					return RedirectToAction("Index", "Category");
				}

				TempData["SuccessMessage"] = "Ürün başarıyla silindi!";
				return RedirectToAction("Index", "Category");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
