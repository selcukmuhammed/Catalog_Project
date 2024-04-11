using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCatalog.Models;
using MVCatalog.Services;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net;

namespace MVCatalog.Controllers
{
	public class ProductController : Controller
	{
		private readonly ProductService _product;
		private readonly CategoryService _category;
		private readonly IHttpContextAccessor _httpContext;

		public ProductController(IConfiguration config, IHttpContextAccessor httpContext)
		{
			_product = new ProductService(config,httpContext);
			_category = new CategoryService(config, httpContext);
			_httpContext = httpContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var token = _httpContext.HttpContext.Session.GetString("token");
			var response = await _product.GetAllProductsAsync();

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
				return RedirectToAction("Index", "Product");
			}
			if (response.StatusCode == "403")
			{
				TempData["ErrorMessage"] = "Hata kodu:403. Bu alana giriş yetkiniz yoktur.";
				return RedirectToAction("Index", "Login");
			}

			if (response.Result == null)
			{
				TempData["ErrorMessage"] = "Ürünler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Product");
			}

			return View(response.Result);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			ProductViewModel product = new ProductViewModel();
			var categoriesResponse = await _category.GetAllCategoryAsync();

			if (categoriesResponse.Result != null)
			{
				var model = (product, categoriesResponse.Result);
				return View(model);
			}
			else
			{
				var model = (product, new List<CategoryViewModel>());
				return View(model);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Add(ProductViewModel product)
		{
			var token = _httpContext.HttpContext.Session.GetString("token");
			var response = await _product.AddProductAsync(product);

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
				return RedirectToAction("Index", "Product");
			}
			if (response.StatusCode == "403")
			{
				TempData["ErrorMessage"] = "Hata kodu:403. Bu alana giriş yetkiniz yoktur.";
				return RedirectToAction("Index", "Login");
			}

			if (response.Result == null)
			{
				TempData["ErrorMessage"] = "Ürünler alınırken bir hata oluştu.";
				return RedirectToAction("Index", "Product");
			}
			if (response.Result)
			{
				TempData["SuccessMessage"] = "Ürünler başarıyla eklendi!";
				return RedirectToAction("Index");
			}
			return RedirectToAction("Add");
		}

		[HttpGet]
		public async Task<IActionResult> Update(long id)
		{
			var response = await _product.GetProductByIdAsync(id);
			var categoriesResponse = await _category.GetAllCategoryAsync();

			if (categoriesResponse.Result != null)
			{
				response.CategoryList = categoriesResponse.Result;
			}
			else
			{
				response.CategoryList = new List<CategoryViewModel>();
			}
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Update(ProductViewModel product)
		{
			try
			{
				var token = _httpContext.HttpContext.Session.GetString("token");
				var response = await _product.UpdateProductAsync(product);

				if (response.StatusCode == "401")
				{
					TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "200")
				{
					TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Product");
				}

				if (response.StatusCode == "403")
				{
					TempData["ErrorMessage"] = "Hata kodu: 403. Bu alana giriş yetkiniz yoktur.";
					return RedirectToAction("Index", "Login");
				}

				if (!response.Result)
				{
					TempData["ErrorMessage"] = "Ürün güncellenirken bir hata oluştu.";
					return RedirectToAction("Index", "Product");
				}

				TempData["SuccessMessage"] = "Ürün başarıyla güncellendi!";
				return RedirectToAction("Index", "Product");
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
				var response = await _product.DeleteProductAsync(id);

				if (response.StatusCode == "401")
				{
					TempData["ErrorMessage"] = "Oturumunuzun süresi dolmuş veya oturumunuz yok. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Login");
				}

				if (response.StatusCode == "200")
				{
					TempData["ErrorMessage"] = "Token doğrulama hatası. Lütfen tekrar giriş yapın.";
					return RedirectToAction("Index", "Product");
				}

				if (response.StatusCode == "403")
				{
					TempData["ErrorMessage"] = "Hata kodu: 403. Bu alana giriş yetkiniz yoktur.";
					return RedirectToAction("Index", "Login");
				}

				if (!response.Result)
				{
					TempData["ErrorMessage"] = "Ürün silinirken bir hata oluştu.";
					return RedirectToAction("Index", "Product");
				}

				TempData["SuccessMessage"] = "Ürün başarıyla silindi!";
				return RedirectToAction("Index", "Product");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
