using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Interfaces;
using TestDemo.Models;

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

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategory()
        {
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

        [HttpPost]
        public async Task<ActionResult<bool>> AddCategory(Category cate)
        {
            bool result = await _categoryService.AddCategory(cate);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> UpdateCategory(Category request)
        {
            try
            {
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

        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(long id)
        {
            try
            {
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
