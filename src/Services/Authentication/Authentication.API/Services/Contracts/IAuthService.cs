using Authentication.API.Enums;
using Authentication.API.Models.Dtos.Auth;

namespace Authentication.API.Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> UserRegisterAsync(RegisterDto registerUser, Role role);
        Task<string> UserLoginAsync(LoginDto loginUser);
    }
}
