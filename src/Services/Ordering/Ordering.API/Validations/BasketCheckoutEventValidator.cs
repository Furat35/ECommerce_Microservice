using EventBus.Message.Events;
using FluentValidation;

namespace Ordering.API.Validations
{
    public class BasketCheckoutEventValidator : AbstractValidator<BasketCheckoutEvent>
    {
        public BasketCheckoutEventValidator()
        {
            RuleFor(_ => _.UserId)
                .NotEmpty()
                .WithMessage("Kullanıcı id'si boş olamaz!");

            RuleFor(_ => _.TotalPrice)
                .NotEmpty()
                .WithMessage("Toplam tutar boş olamaz!");

            RuleFor(_ => _.Name)
               .NotEmpty()
               .WithMessage("İsim boş olamaz!");

            RuleFor(_ => _.Surname)
               .NotEmpty()
               .WithMessage("Soyad boş olamaz!");

            RuleFor(_ => _.Mail)
               .NotEmpty()
               .WithMessage("Mail boş olamaz!");

            RuleFor(_ => _.Address)
               .NotEmpty()
               .WithMessage("Adres boş olamaz!");

            RuleFor(_ => _.PaymentCard)
               .NotEmpty()
               .WithMessage("Ödeme kartı bilgileri boş olamaz!");

            RuleFor(_ => _.OrderItems)
               .NotEmpty()
               .WithMessage("Sipariş ürünleri boş olamaz!");
        }
    }
}
