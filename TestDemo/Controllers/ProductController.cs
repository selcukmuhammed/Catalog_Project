using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCatalog.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestDemo.Interfaces;
using TestDemo.Services;

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

		[HttpGet, Authorize(Roles = "ADMIN, OWNER, USER")]
		public async Task<ActionResult<List<Product>>> GetAllProducts()
		{
			if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
			{
				return Unauthorized("ilk aşamada patladık");
			}
			var token = authHeaderValue.ToString().Replace("Bearer ", "");

			if (!TokenValidator.IsTokenValid(token))
			{
				return Unauthorized("ikinci aşamada patladık.");
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

			if (!TokenValidator.IsTokenValid(token))
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

				if (!TokenValidator.IsTokenValid(token))
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

				if (!TokenValidator.IsTokenValid(token))
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
