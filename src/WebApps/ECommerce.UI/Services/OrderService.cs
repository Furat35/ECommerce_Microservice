using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models.Dtos.ViewModels.Orders;
using ECommerce.UI.Services.Contracts;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public OrderService(HttpClient client, IHttpContextAccessor httpContext)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = httpContext;
        }

        public async Task<(IEnumerable<OrderListModel> products, Metadata pagination)> GetOrders(OrderRequestFilter filters)
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"/Order?Page={filters.Page}&PageSize={filters.PageSize}");

            var pagination = JsonSerializer.Deserialize<Metadata>(response.Headers.FirstOrDefault(_ => _.Key == "X-Pagination").Value.FirstOrDefault());
            return (await response.Content.ReadFromJsonAsync<List<OrderListModel>>(), pagination);
        }

        public async Task<OrderListModel> GetOrderById(string orderId)
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"/Order/{orderId}");

            return await response.Content.ReadFromJsonAsync<OrderListModel>();
        }

        private void AddAuthorizationHeader()
        {
            var claimsIdentity = _context.HttpContext.User.Identity as ClaimsIdentity;
            var bearerTokenClaim = claimsIdentity?.FindFirst("BearerToken");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenClaim.Value);
        }
    }
}
