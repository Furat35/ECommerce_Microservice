using Basket.API.Entities;
using FluentValidation;

namespace Basket.API.Validations
{
    public class ShoppingCartItemValidator : AbstractValidator<ShoppingCartItem>
    {
        public ShoppingCartItemValidator()
        {
            RuleFor(_ => _.Quantity)
                .GreaterThan(0)
                .WithMessage("Adet bilgisi 0'dan büyük olmalıdır!");

            RuleFor(_ => _.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat 0'dan büyük olmalıdır!");

            RuleFor(_ => _.ProductId)
                .NotEmpty()
                .WithMessage("Ürün id'si boş olamaz!");

            RuleFor(_ => _.ProductName)
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz!")
                .MinimumLength(1)
                .WithMessage("Ürün adı en az 1 karakter içermelidir!");
        }
    }
}
