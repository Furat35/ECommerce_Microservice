using Authentication.API.DataAccess.Repositories.Common;
using Authentication.API.Entities;
using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;
using Authentication.API.Models.Dtos.User;
using Authentication.API.Models.Dtos.Users;

namespace Authentication.API.Services.Contracts
{
    public interface IUserService : IAsyncRepository<User>
    {
        Task<UserListDto> GetUserByIdAsync(string userId);
        Task<User> GetUserByMailAsync(string mail);
        Task<bool> CheckIfUserExists(string email);
        Task<bool> UpdateUserPasswordAsync(string userId, string password);
        Task<bool> UpdateAddress(AddressAddDto address);
        Task<bool> UpdatePaymentCard(PaymentCardAddDto paymentCard);
        Task<bool> SafeDeleteUserAsync(string userId);
        Task<bool> AddUserAsync(UserAddDto user);
    }
}
