using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models.Products;
using Shared.Helpers;

namespace Catalog.API.Helpers.Filters.Products
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
            _products = OrderProductsByDate(filters.DateDesc);

            int pageNumber = _products.Count() % filters.PageSize == 0 ? _products.Count() / filters.PageSize : _products.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _products.Count(), pageNumber);
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
            ? _products.Where(category => category.Name.ToUpper().StartsWith(productName.ToUpper())).ToList()
            : _products;

        private List<Product> OrderProductsByDate(bool isDesc)
            => isDesc
            ? _products.OrderByDescending(_ => _.CreatedDate).ToList()
            : _products.OrderBy(_ => _.CreatedDate).ToList();


        private List<Product> AddPagination(ProductRequestFilter filters)
          => _products
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize)
              .ToList();
    }
}
