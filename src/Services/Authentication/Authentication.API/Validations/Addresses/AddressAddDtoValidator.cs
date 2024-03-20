using Authentication.API.Models.Dtos.Addresses;
using FluentValidation;

namespace Authentication.API.Validations.Addresses
{
    public class AddressAddDtoValidator : AbstractValidator<AddressAddDto>
    {
        public AddressAddDtoValidator()
        {
            RuleFor(_ => _.AddressLine)
                .NotEmpty()
                .WithMessage("Address alanı boş olamaz!");

            RuleFor(_ => _.Country)
                .NotEmpty()
                .WithMessage("Ülke alanı boş olamaz!");

            RuleFor(_ => _.State)
                .NotEmpty()
                .WithMessage("Şehir alanı boş olamaz!");

            RuleFor(_ => _.ZipCode)
                .NotEmpty()
                .WithMessage("Alan kodu alanı boş olamaz!");
        }
    }
}
