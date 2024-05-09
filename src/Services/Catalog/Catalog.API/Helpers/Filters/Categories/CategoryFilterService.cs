using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models.Categories;
using Shared.Helpers;

namespace Catalog.API.Helpers.Filters.Categories
{
    public class CategoryFilterService
    {
        private readonly IMapper _mapper;
        private List<Category> _categories;

        public CategoryFilterService(IMapper mapper, List<Category> Categories)
        {
            _mapper = mapper;
            _categories = Categories;
        }

        public CategoryResponse<List<CategoryListDto>> FilterCategories(CategoryRequestFilter filters)
        {
            int pageNumber = _categories.Count() % filters.PageSize == 0 ? _categories.Count() / filters.PageSize : _categories.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _categories.Count(), pageNumber);
            _categories = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedCategories = _mapper.Map<List<CategoryListDto>>(_categories);

            return new()
            {
                ResponseValue = mappedCategories,
                Headers = header
            };
        }

        private List<Category> AddPagination(CategoryRequestFilter filters)
          => _categories
              .OrderBy(_ => _.Name)
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize)
              .ToList();
    }
}
