using Authentication.API.Models.Dtos.Users;
using FluentValidation;

namespace Authentication.API.Validations.Users
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("İsim alanını doldurunuz!")
                .MaximumLength(50)
                .WithMessage("İsim en fazla 50 karakter içerebilir!");

            RuleFor(_ => _.Surname)
                .NotEmpty()
                .WithMessage("Soyad alanını doldurunuz!")
                .MaximumLength(50)
                .WithMessage("Soyad en fazla 50 karakter içerebilir!");

            RuleFor(_ => _.Mail)
                .NotEmpty()
                .WithMessage("Mail alanını doldurunuz!")
                .MaximumLength(50)
                .WithMessage("Mail en fazla 50 karakter içerebilir!");
        }
    }
}
