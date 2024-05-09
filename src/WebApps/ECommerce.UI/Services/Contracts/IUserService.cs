using ECommerce.UI.Models.Dtos.Addresses;
using ECommerce.UI.Models.Dtos.PaymentCards;
using ECommerce.UI.Models.Dtos.ViewModels.Users;

namespace ECommerce.UI.Services.Contracts
{
    public interface IUserService
    {
        Task<UserListModel> GetUserById(string userId);
        Task<bool> UpdateAddress(AddressAddDto model);
        Task<bool> UpdatePaymentCard(PaymentCardAddDto model);
        Task<bool> UpdateData(UserDataUpdateModel userData);
        Task<bool> UpdatePassword(string newPassword);
        Task<bool> DeleteUser(string userId);
    }
}
