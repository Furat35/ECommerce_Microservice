using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Products;

namespace ECommerce.UI.Services.Contracts
{
    public interface ICatalogService
    {
        Task<(IEnumerable<ProductListModel> products, Metadata pagination)> GetProducts(ProductRequestFilter filters);
        Task<ProductListModel> GetProductById(string id);
        Task<(ProductListWithCategoryModel products, Metadata pagination)> GetProductsByCategoryId(ProductRequestFilter filters);
        Task<string> CreateProduct(ProductCreateModel model);
        Task<bool> UpdateProduct(ProductUpdateModel model);
        Task<bool> UpdateProductPhoto(IFormFile file, string productId);
        Task<bool> DeleteProduct(string productId);
    }
}
