using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
using MVCatalog.Services;
using NuGet.Common;

namespace MVCatalog.Controllers
{
	public class LoginController : Controller
	{
		private readonly AuthService _authService;
		private readonly IHttpContextAccessor _httpContext;
        public LoginController(IConfiguration config, IHttpContextAccessor httpContext)
        {
			_authService = new AuthService(config);
			_httpContext = httpContext;
        }

		[HttpGet]
        public  IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Index(LoginModel loginModel)
		{
			var token = await _authService.LoginAsync(loginModel);
			

			if (token != null)
			{
				_httpContext.HttpContext.Session.SetString("token", token);
				return RedirectToAction("Index", "Product");
			}
			else
			{
				ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _authService.LogoutAsync();
			return RedirectToAction("Index", "Login");
		}
	}
}
