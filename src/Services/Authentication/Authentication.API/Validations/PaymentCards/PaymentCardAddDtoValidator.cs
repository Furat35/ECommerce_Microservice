using Authentication.API.Models.Dtos.PaymentCards;
using FluentValidation;

namespace Authentication.API.Validations.PaymentCards
{
    public class PaymentCardAddDtoValidator : AbstractValidator<PaymentCardAddDto>
    {
        public PaymentCardAddDtoValidator()
        {
            RuleFor(_ => _.CardName)
                .NotEmpty()
                .WithMessage("Kart ismi boş olamaz");

            RuleFor(_ => _.CardNumber)
                .NotEmpty()
                .WithMessage("Kart numarası boş olamaz");

            RuleFor(_ => _.Expiration)
                .NotEmpty()
                .WithMessage("Bitiş tarihi boş olamaz");

            RuleFor(_ => _.CVV)
                .NotEmpty()
                .WithMessage("CVV boş olamaz");
        }
    }
}
