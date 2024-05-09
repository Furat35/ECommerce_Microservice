using Catalog.API.Models.Products;
using FluentValidation;

namespace Catalog.API.Validations.Products
{
    public class ProductAddDtoValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddDtoValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz!");

            RuleFor(_ => _.CategoryId)
               .NotEmpty()
               .WithMessage("Kategori id'si olamaz!");

            RuleFor(_ => _.Description)
               .NotEmpty()
               .WithMessage("Ürün hakkında boş olamaz!");

            RuleFor(_ => _.Summary)
               .NotEmpty()
               .WithMessage("Ürün özeti boş olamaz!");

            //RuleFor(_ => _.ImageFile)
            //   .NotEmpty()
            //   .WithMessage("Ürün görseli boş olamaz!");

            RuleFor(_ => _.Price)
               .GreaterThan(0)
               .WithMessage("Ürün fiyatı 0'dan büyük olmalıdır!");
        }
    }
}
