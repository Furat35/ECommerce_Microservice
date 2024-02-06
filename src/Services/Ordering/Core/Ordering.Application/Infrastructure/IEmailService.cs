using Ordering.Application.Models;

namespace Ordering.Application.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
