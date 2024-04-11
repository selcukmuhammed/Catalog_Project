using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCatalog.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestDemo.Interfaces;

namespace TestDemo.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		//handler
		private bool IsTokenValid(string token)

		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("951CC7D8-C2A7-42D4-8D99-3DA0345EFBA9")),
				ValidateIssuer = false,
				ValidateAudience = false
			};

			try
			{
				SecurityToken validatedToken;
				var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

				// Token içerisindeki "JWTID" claim'ine erişme
				var jwtIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "JWTID");

				// Eğer "JWTID" claim'i varsa ve değeri doğruysa true döndür
				if (jwtIdClaim != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				// Token doğrulama sırasında hata oluşursa
				return false;
			}
		}

		[HttpGet, Authorize(Roles = "ADMIN, OWNER, USER")]
		public async Task<ActionResult<List<Product>>> GetAllProducts()
		{
			if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
			{
				return Unauthorized("ilk aşamada patladık"); // Eğer Authorization başlığı yoksa, yetkisiz erişim hatası döndürün
			}
			var token = authHeaderValue.ToString().Replace("Bearer ", ""); // Bearer 'ı kaldırarak sadece token'i alın

			if (!IsTokenValid(token))
			{
				return Unauthorized("ikinci aşamada patladık."); // Eğer token geçerli değilse, yetkisiz erişim hatası döndürün
			}
			return await _productService.GetAllProducts();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductById(long id)
		{
			var result = await _productService.GetProductById(id);
			if (result is null)
				return NotFound("Ürün bulunamadı.");

			return Ok(result);
		}
		[HttpPost, Authorize(Roles = "ADMIN")]
		public async Task<ActionResult<bool>> AddProduct(ProductDto prdct)
		{
			if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
			{
				return Unauthorized("ilk aşamada patladık");
			}
			var token = authHeaderValue.ToString().Replace("Bearer ", "");

			if (!IsTokenValid(token))
			{
				return Unauthorized("ikinci aşamada patladık.");
			}

			bool result = await _productService.AddProduct(prdct);
			return Ok(result);
		}
		[HttpPost, Authorize(Roles = "ADMIN,OWNER")]
		public async Task<ActionResult<bool>> UpdateProduct(ProductDto request)
		{
			try
			{
				if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
				{
					return Unauthorized("ilk aşamada patladık");
				}
				var token = authHeaderValue.ToString().Replace("Bearer ", "");

				if (!IsTokenValid(token))
				{
					return Unauthorized("ikinci aşamada patladık.");
				}

				bool result = await _productService.UpdateProduct(request);
				if (!result)
					return false;

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}
		[HttpPost("{id}"), Authorize(Roles = "ADMIN,OWNER")]
		public async Task<ActionResult<bool>> DeleteProduct(long id)
		{
			try
			{
				if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
				{
					return Unauthorized("ilk aşamada patladık");
				}
				var token = authHeaderValue.ToString().Replace("Bearer ", "");

				if (!IsTokenValid(token))
				{
					return Unauthorized("ikinci aşamada patladık.");
				}

				bool result = await _productService.DeleteProduct(id);
				if (!result)
					return false;

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
