
using MVCatalog.Models;
using TestDemo.Models;

namespace TestDemo.Services
{
    public class ProductService : IProductService
    {

        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(ProductDto pro)
        {
            Product product = new Product()
            {
                Name = pro.Name,
                Price = pro.Price,
                IsDeleted = pro.IsDeleted,
                RecordTime = pro.RecordTime,
                CategoryId = pro.CategoryId,
                Categories = null
			};

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var pro = await _context.Products.FindAsync(id);
            if (pro is null)
                return false;

            _context.Products.Remove(pro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetProductById(long id)
        {
            var pro = await _context.Products.FindAsync(id);
            if (pro is null)
                return null;

            return pro;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _context.Products.Include(x => x.Categories).ToListAsync();
            return products;
        }

        public async Task<bool> UpdateProduct(ProductDto request)
        {
            var pro = await _context.Products.FindAsync(request.ProductId);
            if (pro is null)
                return false;

            pro.Name = request.Name;
            pro.Price = request.Price;
            pro.IsDeleted = request.IsDeleted;
            pro.RecordTime = request.RecordTime;
            pro.CategoryId = request.CategoryId;
            pro.Categories = null;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
