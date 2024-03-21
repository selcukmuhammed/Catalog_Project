using MVCatalog.Models;

namespace TestDemo.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(long id);
        Task<bool> AddProduct(ProductDto pro);
        Task<bool> UpdateProduct(ProductDto request);
        Task<bool> DeleteProduct(long id);

    }
}
