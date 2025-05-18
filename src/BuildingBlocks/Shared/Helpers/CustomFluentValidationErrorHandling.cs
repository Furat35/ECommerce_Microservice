
using FluentValidation;
using Shared.Exceptions;
using Shared.Helpers.interfaces;

namespace Shared.Helpers
{
    public class CustomFluentValidationErrorHandling(IServiceProvider serviceProvider) : ICustomFluentValidationErrorHandling
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task ValidateAndThrowAsync<T>(T input)
        {
            var validator = _serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T>;
            if (validator is null)
                throw new NotFoundException($"Validator not found for type {validator.GetType().Name}");

            var validationResult = await validator.ValidateAsync(input);
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
        }
    }
}
