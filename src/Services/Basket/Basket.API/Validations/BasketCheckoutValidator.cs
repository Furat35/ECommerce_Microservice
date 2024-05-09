using Basket.API.Entities;
using FluentValidation;

namespace Basket.API.Validations
{
    public class BasketCheckoutValidator : AbstractValidator<BasketCheckout>
    {
        public BasketCheckoutValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Ad boş olamaz!");

            RuleFor(_ => _.Surname)
                .NotEmpty()
                .WithMessage("Soyad boş olamaz!");

            RuleFor(_ => _.Address)
                .NotEmpty()
                .WithMessage("Adres boş olamaz!");

            RuleFor(_ => _.PaymentCard)
                .NotEmpty()
                .WithMessage("Ödeme bilgisi boş olamaz!");
        }
    }
}
