using Authentication.API.Models.Dtos.Auth;
using FluentValidation;

namespace Authentication.API.Validations.Auths
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("İsim alanı boş olamaz!");

            RuleFor(_ => _.Surname)
               .NotEmpty()
               .WithMessage("Soyad alanı boş olamaz!");

            RuleFor(_ => _.Mail)
               .NotEmpty()
               .WithMessage("Mail alanı boş olamaz!");

            RuleFor(_ => _.Password)
               .NotEmpty()
               .WithMessage("Şifre alanı boş olamaz!");
        }
    }
}
