namespace Shared.Helpers.interfaces
{
    public interface ICustomFluentValidationErrorHandling
    {
        Task ValidateAndThrowAsync<T>(T input);
    }
}
