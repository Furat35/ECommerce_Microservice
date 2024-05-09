using Catalog.API.Entities;
using Catalog.API.Helpers.Filters.Categories;
using Catalog.API.Models.Categories;

namespace Catalog.API.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<CategoryListDto>> GetCategoriesAsync(CategoryRequestFilter filters);
        Task<CategoryListDto> GetCategoryAsync(string id);
        Task<Category> GetSingleCategoryAsync(string id);
        Task<CategoryListDto> GetCategoryByNameAsync(string name);
        Task CreateCategoryAsync(CategoryAddDto Category);
        Task<bool> UpdateCategoryAsync(CategoryUpdateDto Category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
