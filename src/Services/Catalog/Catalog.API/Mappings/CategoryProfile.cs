using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Models.Categories;

namespace Catalog.API.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryListDto>();
            CreateMap<CategoryAddDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
