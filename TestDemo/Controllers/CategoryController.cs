using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestDemo.Interfaces;
using TestDemo.Models;
using TestDemo.Services;

namespace TestDemo.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet, Authorize(Roles = "ADMIN,USER,OWNER")]
		public async Task<ActionResult<List<Category>>> GetAllCategory()
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
			return await _categoryService.GetAllCategory();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> GetCategoryById(long id)
		{
			var result = await _categoryService.GetCategoryById(id);
			if (result is null)
				return NotFound("Ürün bulunamadı");

			return Ok(result);
		}

		[HttpPost, Authorize(Roles = "ADMIN,OWNER")]
		public async Task<ActionResult<bool>> AddCategory(Category cate)
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

			bool result = await _categoryService.AddCategory(cate);
			return Ok(result);
		}

		[HttpPost, Authorize(Roles = "ADMIN,OWNER")]
		public async Task<ActionResult<bool>> UpdateCategory(Category request)
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

				bool result = await _categoryService.UpdateCategory(request);
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
		public async Task<ActionResult<bool>> DeleteCategory(long id)
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

				bool result = await _categoryService.DeleteCategory(id);
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
