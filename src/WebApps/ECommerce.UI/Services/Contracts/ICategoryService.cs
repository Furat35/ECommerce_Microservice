using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Categories;

namespace ECommerce.UI.Services.Contracts
{
    public interface ICategoryService
    {
        Task<List<CategoryListModel>> GetCategories();
        Task<(List<CategoryListModel> categories, Metadata pagination)> GetCategories(CategoryRequestFilter filters);
        Task CreateCategory(CategoryCreateModel model);
        Task<bool> UpdateCategory(CategoryUpdateModel model);
        Task<bool> DeleteCategory(string categoryId);
    }
}
