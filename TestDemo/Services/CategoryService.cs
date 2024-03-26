
using TestDemo.Interfaces;

namespace TestDemo.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCategory(Category cate)
        {
            _context.Categories.Add(cate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategory(long id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate is null)
                return false;

            _context.Categories.Remove(cate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryById(long id)
        {
            var cate = await _context.Categories.FindAsync(id);
            if (cate is null)
                return null;

            return cate;
        }

        public async Task<bool> UpdateCategory(Category request)
        {
            var cate = await _context.Categories.FindAsync(request.CategoryId);
            if (cate is null)
                return false;

            cate.Name = request.Name;
            cate.RecordTime = request.RecordTime;
            cate.IsDeleted = request.IsDeleted;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
