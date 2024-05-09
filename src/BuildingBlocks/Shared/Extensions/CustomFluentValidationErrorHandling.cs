
using FluentValidation;
using Shared.Exceptions;

namespace Shared.Extensions
{
    public static class CustomFluentValidationErrorHandling
    {
        public static async Task ValidateAndThrowAsync<T>(T input, IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(input);
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
        }
    }
}
