using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Helpers;
using Catalog.API.Helpers.Filters;
using Catalog.API.Models.Product;
using MongoDB.Driver;
using Shared.Exceptions;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ProductRepository(ICatalogContext catalogContext, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _catalogContext = catalogContext;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public async Task CreateProductAsync(ProductAddDto product)
        {
            var productToAdd = _mapper.Map<Product>(product);
            await _catalogContext.Products.InsertOneAsync(productToAdd);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var deleteResult = await _catalogContext
                           .Products
                           .DeleteOneAsync(p => p.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<ProductListDto> GetProductAsync(string id)
        {
            var product = await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product is null)
                throw new NotFoundException();

            return _mapper.Map<ProductListDto>(product);
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(string categoryName, ProductRequestFilter filters)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter
                    .Where(p => p.Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            var products = await _catalogContext
                .Products
                .Find(filter)
                .ToListAsync();


            var filteredProducts = new ProductFilterService(_mapper, products).FilterProducts(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredProducts.Headers);

            return filteredProducts.ResponseValue;

        }

        public async Task<IEnumerable<ProductListDto>> GetProductByNameAsync(string name)
        {
            var products = await _catalogContext
                .Products
                .Find(p => p.Name == name)
                .ToListAsync();

            return _mapper.Map<List<ProductListDto>>(products);
        }

        public async Task<List<ProductListDto>> GetProductsAsync(ProductRequestFilter filters)
        {
            var products = await _catalogContext
                .Products
                .Find(p => true)
                .ToListAsync();

            var filteredProducts = new ProductFilterService(_mapper, products).FilterProducts(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredProducts.Headers);

            return filteredProducts.ResponseValue;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            var productToUpdate = await _catalogContext.Products.Find(p => p.Id == product.Id).FirstOrDefaultAsync();
            _mapper.Map(product, productToUpdate);
            var updateResult = await _catalogContext
                .Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: productToUpdate);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
