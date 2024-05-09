using ECommerce.UI.Extensions;
using ECommerce.UI.Models.Dtos.ViewModels.Baskets;
using ECommerce.UI.Services.Contracts;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ECommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public BasketService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = httpContextAccessor;
        }

        public async Task<ShoppingCartModel> GetBasket()
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"/Basket");
            return await response.ReadContentAs<ShoppingCartModel>();
        }

        public async Task<ShoppingCartModel> UpdateBasket(ShoppingCartModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson($"/Basket", model);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ShoppingCartModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<ShoppingCartModel> RefreshBasket()
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJson<string>($"/Basket/Refresh", null);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ShoppingCartModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<ShoppingCartModel> RemoveItemFromBasket(string productId)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"/Basket/{productId}");
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ShoppingCartModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task<ShoppingCartModel> DecreaseItemQuantity(string productId)
        {
            AddAuthorizationHeader();
            var response = await _client.PostAsJson<string>($"/Basket/DecreaseItemQuantity/{productId}", null);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ShoppingCartModel>();
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PostAsJson($"/Basket/Checkout", model);
            if (!response.IsSuccessStatusCode)
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
