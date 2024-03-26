using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
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

        [HttpGet]
        public async Task<List<Product>> GetAllProducts()
        {
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
        [HttpPost]
        public async Task<ActionResult<bool>> AddProduct(ProductDto prdct)
        {
            bool result = await _productService.AddProduct(prdct);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<bool>> UpdateProduct(ProductDto request)
        {
            try
            {
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
        [HttpPost("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(long id)
        {
            try
            {
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
