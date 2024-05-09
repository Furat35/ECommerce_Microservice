using Catalog.API.Helpers.Filters.Products;
using Catalog.API.Models.Products;

namespace Catalog.API.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<List<ProductListDto>> GetProductsAsync(ProductRequestFilter filters);
        Task<ProductListDto> GetProductAsync(string id);
        Task<ProductListWithCategoryDto> GetProductsByCategoryAsync(ProductRequestFilter filters);
        Task<string> CreateProductAsync(ProductAddDto product);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> UpdateProductPhoto(string id);
        Task<bool> DeleteProductAsync(string id);
    }
}
