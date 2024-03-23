using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models.Product;

namespace Catalog.API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductListDto>();
            CreateMap<ProductAddDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
