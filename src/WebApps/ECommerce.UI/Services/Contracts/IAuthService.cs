using ECommerce.UI.Models.Dtos.Auth;

namespace ECommerce.UI.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto loginDto);
        Task SignOutAsync();
        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}
