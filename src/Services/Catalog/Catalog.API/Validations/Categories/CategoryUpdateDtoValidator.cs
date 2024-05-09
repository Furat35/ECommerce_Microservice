using Catalog.API.Models.Categories;
using FluentValidation;

namespace Catalog.API.Validations.Categories
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(_ => _.CategoryId)
                .NotEmpty()
                .WithMessage("Kategori id'si giriniz!");

            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Kategori adı giriniz!");
        }
    }
}
