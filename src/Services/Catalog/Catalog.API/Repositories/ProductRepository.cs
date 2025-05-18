using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.GrpcServices;
using Catalog.API.Helpers.Filters.Products;
using Catalog.API.Models.Products;
using Catalog.API.Repositories.Contracts;
using Catalog.API.Services.Contracts;
using MongoDB.Driver;
using Shared.Exceptions;
using Shared.Helpers;
using Shared.Helpers.interfaces;

namespace Catalog.API.Repositories
{
    public class ProductRepository(ICatalogContext catalogContext, IHttpContextAccessor httpContext, IMapper mapper, IProductPhotoService productPhotoService,
        ICustomFluentValidationErrorHandling customValidator, ICategoryRepository categoryRepository,
        IFileService fileService, DiscountGrpcService discountGrpcService) : IProductRepository
    {
        private readonly ICatalogContext _catalogContext = catalogContext;
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly IMapper _mapper = mapper;
        private readonly IProductPhotoService _productPhotoService = productPhotoService;
        private readonly ICustomFluentValidationErrorHandling _customValidator = customValidator;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IFileService _fileService = fileService;
        private readonly DiscountGrpcService _discountGrpcService = discountGrpcService;

        public async Task<string> CreateProductAsync(ProductAddDto product)
        {
            await _customValidator.ValidateAndThrowAsync(product);
            await _categoryRepository.GetCategoryAsync(product.CategoryId); // throws error if category doesn't exist
            var productToAdd = _mapper.Map<Product>(product);
            productToAdd.CreatedDate = DateTime.UtcNow;
            productToAdd.CreatedBy = _httpContext.HttpContext.User.GetActiveUserId();
            productToAdd.ImageFile = await _productPhotoService.UploadProductPhoto(productToAdd);
            await _catalogContext.Products.InsertOneAsync(productToAdd);

            return productToAdd.Id;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var product = await GetProductById(id);
            var deleteResult = await _catalogContext
                           .Products
                           .DeleteOneAsync(p => p.Id == id);
            _productPhotoService.RemoveProductPhoto(product);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProductPhoto(string id)
        {
            var product = await GetProductById(id);
            product.ImageFile = await _productPhotoService.UploadProductPhoto(product);
            var updateResult = await _catalogContext
                .Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged;
        }

        public async Task<ProductListDto> GetProductAsync(string id)
        {
            var product = await GetProductById(id);
            product.Category = await _categoryRepository.GetSingleCategoryAsync(product.CategoryId);
            product.ImageFile = GetProductPhoto(product.ImageFile);
            var productList = _mapper.Map<ProductListDto>(product);
            productList.Discount = (await _discountGrpcService.GetDiscount(id)).Amount;

            return productList;
        }

        public async Task<ProductListWithCategoryDto> GetProductsByCategoryAsync(ProductRequestFilter filters)
        {
            var category = await _categoryRepository.GetCategoryAsync(filters.CategoryId);
            var products = await _catalogContext
                .Products
                .Find(p => p.CategoryId == filters.CategoryId)
                .ToListAsync();

            var filteredProducts = new ProductFilterService(_mapper, products).FilterProducts(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredProducts.Headers);
            foreach (var product in filteredProducts.ResponseValue)
            {
                product.ImageFile = GetProductPhoto(product.ImageFile);
                product.Discount = (await _discountGrpcService.GetDiscount(product.Id)).Amount;
            }

            return new ProductListWithCategoryDto { CategoryId = category.CategoryId, CategoryName = category.Name, Products = filteredProducts.ResponseValue };
        }

        public async Task<List<ProductListDto>> GetProductsAsync(ProductRequestFilter filters)
        {
            var products = await _catalogContext
                .Products
                .Find(p => true)
                .ToListAsync();
            await SetProductCategories(products);

            var filteredProducts = new ProductFilterService(_mapper, products).FilterProducts(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredProducts.Headers);
            foreach (var product in filteredProducts.ResponseValue)
            {
                product.ImageFile = GetProductPhoto(product.ImageFile);
                product.Discount = (await _discountGrpcService.GetDiscount(product.Id)).Amount;
            }

            return filteredProducts.ResponseValue;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            await _customValidator.ValidateAndThrowAsync(product);
            var productToUpdate = await GetProductById(product.Id);
            _mapper.Map(product, productToUpdate);
            var updateResult = await _catalogContext
                .Products
                .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: productToUpdate);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        private async Task<Product> GetProductById(string productId)
        {
            var product = await _catalogContext.Products.Find(p => p.Id == productId).FirstOrDefaultAsync();
            if (product is null)
                throw new NotFoundException("Ürün bulunamadı!");

            return product;
        }

        private async Task SetProductCategories(List<Product> products)
        {
            foreach (var product in products)
                product.Category = await _categoryRepository.GetSingleCategoryAsync(product.CategoryId);
        }

        private string GetProductPhoto(string imagePath)
        {
            try
            {
                return _fileService.GetImage(imagePath);
            }
            catch (Exception)
            {
                // todo : log can be added
            }

            return null;
        }
    }
}
