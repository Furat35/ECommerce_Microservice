using Authentication.API.Models.Dtos.Auth;
using FluentValidation;

namespace Authentication.API.Validations.Auths
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(_ => _.Mail)
                .NotEmpty()
                .WithMessage("Mail alanı boş olamaz!");

            RuleFor(_ => _.Password)
                .NotEmpty()
                .WithMessage("Şifre alanı boş olamaz!");
        }
    }
}
