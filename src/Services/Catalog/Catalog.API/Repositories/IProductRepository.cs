using Catalog.API.Helpers.Filters;
using Catalog.API.Models.Product;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductListDto>> GetProductsAsync(ProductRequestFilter filters);
        Task<ProductListDto> GetProductAsync(string id);
        Task<IEnumerable<ProductListDto>> GetProductByNameAsync(string name);
        Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(string categoryName, ProductRequestFilter filters);
        Task CreateProductAsync(ProductAddDto product);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> DeleteProductAsync(string id);
    }
}
