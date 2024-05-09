using Discount.API.Entities;
using FluentValidation;

namespace Discount.API.Validations
{
    public class CouponValidator : AbstractValidator<Coupon>
    {
        public CouponValidator()
        {
            RuleFor(_ => _.ProductId)
                .NotEmpty()
                .WithMessage("Ürün id'si boş olamaz!");

            RuleFor(_ => _.Description)
                .NotEmpty()
                .WithMessage("Ürün açıklaması boş olamaz!");

            RuleFor(_ => _.Amount)
                .GreaterThan(0)
                .WithMessage("İndirim tutarı 0'dan büyük olmalıdır!");
        }
    }
}
