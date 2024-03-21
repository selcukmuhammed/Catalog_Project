namespace TestDemo.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategory();
        Task<Category> GetCategoryById(long id);
        Task<bool> AddCategory(Category cate);
        Task<bool> UpdateCategory(Category request);
        Task<bool> DeleteCategory(long id);
    }
}
