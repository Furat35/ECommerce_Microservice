using Catalog.API.Models.Categories;
using FluentValidation;

namespace Catalog.API.Validations.Categories
{
    public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
    {
        public CategoryAddDtoValidator()
        {
            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("Kategori adı giriniz!");
        }
    }
}
