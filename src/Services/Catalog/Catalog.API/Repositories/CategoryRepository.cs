using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Helpers.Filters.Categories;
using Catalog.API.Models.Categories;
using Catalog.API.Repositories.Contracts;
using FluentValidation;
using MongoDB.Driver;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;

namespace Catalog.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICatalogContext _catalogContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryAddDto> _categoryAddDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;

        public CategoryRepository(ICatalogContext catalogContext, IHttpContextAccessor httpContext, IMapper mapper,
            IValidator<CategoryAddDto> categoryAddDtoValidator, IValidator<CategoryUpdateDto> categoryUpdateDtoValidator)
        {
            _catalogContext = catalogContext;
            _httpContext = httpContext;
            _mapper = mapper;
            _categoryAddDtoValidator = categoryAddDtoValidator;
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
        }

        public async Task CreateCategoryAsync(CategoryAddDto category)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(category, _categoryAddDtoValidator);
            var categoryToAdd = _mapper.Map<Category>(category);
            await _catalogContext.Categories.InsertOneAsync(categoryToAdd);
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var deleteResult = await _catalogContext
                           .Categories
                           .DeleteOneAsync(filter: _ => _.CategoryId == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<CategoryListDto> GetCategoryAsync(string id)
        {
            var category = await GetCategoryById(id);
            return _mapper.Map<CategoryListDto>(category);
        }

        public async Task<Category> GetSingleCategoryAsync(string id)
            => await GetCategoryById(id);

        public async Task<CategoryListDto> GetCategoryByNameAsync(string name)
        {
            var categories = await _catalogContext
                .Categories
                .Find(p => p.Name == name)
                .ToListAsync();

            return _mapper.Map<CategoryListDto>(categories);
        }

        public async Task<List<CategoryListDto>> GetCategoriesAsync(CategoryRequestFilter filters)
        {
            var categories = await _catalogContext
                .Categories
                .Find(p => true)
                .ToListAsync();

            var filteredCategories = new CategoryFilterService(_mapper, categories).FilterCategories(filters);
            new HeaderService(_httpContext).AddToHeaders(filteredCategories.Headers);

            return filteredCategories.ResponseValue;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateDto category)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(category, _categoryUpdateDtoValidator);
            var categoryToUpdate = await GetCategoryById(category.CategoryId);
            _mapper.Map(category, categoryToUpdate);
            var updateResult = await _catalogContext
                .Categories
                .ReplaceOneAsync(filter: g => g.CategoryId == category.CategoryId, replacement: categoryToUpdate);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }



        private async Task<Category> GetCategoryById(string CategoryId)
        {
            var category = await _catalogContext.Categories.Find(p => p.CategoryId == CategoryId).FirstOrDefaultAsync();
            if (category is null)
                throw new NotFoundException("Kategori bulunamadı!");

            return category;
        }
    }
}
