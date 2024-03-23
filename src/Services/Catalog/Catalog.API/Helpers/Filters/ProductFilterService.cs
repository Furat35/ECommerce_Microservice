using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models.Product;

namespace Catalog.API.Helpers.Filters
{
    public class ProductFilterService
    {
        private readonly IMapper _mapper;
        private List<Product> _products;

        public ProductFilterService(IMapper mapper, List<Product> products)
        {
            _mapper = mapper;
            _products = products;
        }

        public ProductResponse<List<ProductListDto>> FilterProducts(ProductRequestFilter filters)
        {
            _products = NameStartsWith(filters.Name);

            Metadata metadata = new(filters.Page, filters.PageSize, _products.Count(), _products.Count() / filters.PageSize + 1);
            _products = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedProducts = _mapper.Map<List<ProductListDto>>(_products);

            return new()
            {
                ResponseValue = mappedProducts,
                Headers = header
            };
        }

        private List<Product> NameStartsWith(string productName)
         => !string.IsNullOrEmpty(productName)
            ? _products.Where(category => category.Name.StartsWith(productName)).ToList()
            : _products;

        private List<Product> AddPagination(ProductRequestFilter filters)
          => _products
              .OrderBy(_ => _.Name)
              .Skip(filters.Page * filters.PageSize)
              .Take(filters.PageSize)
              .ToList();
    }
}
