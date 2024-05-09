using ECommerce.UI.Extensions;
using ECommerce.UI.Models;
using ECommerce.UI.Models.Dtos.ViewModels.Discounts;
using ECommerce.UI.Services.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.UI.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public DiscountService(HttpClient client, IHttpContextAccessor context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context;
        }

        public async Task<DiscountListModel> GetDiscountByProductId(string id)
        {
            var response = await _client.GetAsync($"/Discount/{id}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.Content.ReadFromJsonAsync<DiscountListModel>();
        }

        public async Task<string> CreateDiscount(DiscountCreateModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PostAsJson($"/Discount", model);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> UpdateDiscount(DiscountUpdateModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJsonAsync($"/Discount", model);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> DeleteDiscount(string discountId)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"/Discount/{discountId}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }

        private async Task ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                var errorDetails = JsonSerializer.Deserialize<ErrorDetail>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                throw new HttpRequestException(message: errorDetails.ErrorMessage, null, statusCode: (HttpStatusCode)(errorDetails.StatusCode));
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
