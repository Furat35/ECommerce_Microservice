using Catalog.API.Models.Products;
using FluentValidation;

namespace Catalog.API.Validations.Products
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(_ => _.Id)
              .NotEmpty()
              .WithMessage("Ürün id'si boş olamaz!");

            RuleFor(_ => _.Name)
              .NotEmpty()
              .WithMessage("Ürün adı boş olamaz!");

            RuleFor(_ => _.CategoryId)
               .NotEmpty()
               .WithMessage("Kategori id'si boş olamaz!");

            RuleFor(_ => _.Description)
               .NotEmpty()
               .WithMessage("Ürün hakkında boş olamaz!");

            RuleFor(_ => _.Summary)
               .NotEmpty()
               .WithMessage("Ürün özeti boş olamaz!");

            RuleFor(_ => _.Price)
               .GreaterThan(0)
               .WithMessage("Ürün fiyatı 0'dan büyük olmalıdır!");
        }
    }
}
