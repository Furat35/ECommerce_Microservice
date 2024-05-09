using Basket.API.Entities;
using FluentValidation;

namespace Basket.API.Validations
{
    public class ShoppingCartValidator : AbstractValidator<ShoppingCart>
    {
        public ShoppingCartValidator()
        {
            RuleFor(_ => _.Items)
                .NotEmpty()
                .WithMessage("Ürün ekleyiniz!");
        }
    }
}
