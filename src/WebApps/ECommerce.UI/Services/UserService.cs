using ECommerce.UI.Extensions;
using ECommerce.UI.Models.Dtos.Addresses;
using ECommerce.UI.Models.Dtos.PaymentCards;
using ECommerce.UI.Models.Dtos.ViewModels.Users;
using ECommerce.UI.Services.Contracts;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ECommerce.UI.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public UserService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = httpContextAccessor;
        }

        public async Task<UserListModel> GetUserById(string userId)
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"/User/{userId}");

            return await response.ReadContentAs<UserListModel>();
        }
        public async Task<bool> UpdateAddress(AddressAddDto model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson($"/User/Address", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<bool> UpdatePaymentCard(PaymentCardAddDto model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson($"/User/PaymentCard", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<bool> UpdateData(UserDataUpdateModel userData)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson($"/User/Data", userData);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<bool> UpdatePassword(string newPassword)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson($"/User", newPassword);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"/User/{userId}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        private void AddAuthorizationHeader()
        {
            var claimsIdentity = _context.HttpContext.User.Identity as ClaimsIdentity;
            var bearerTokenClaim = claimsIdentity?.FindFirst("BearerToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenClaim.Value);
        }
    }
}
