using ECommerce.UI.Extensions;
using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models;
using ECommerce.UI.Models.Dtos.ViewModels.Categories;
using ECommerce.UI.Services.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ECommerce.UI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public CategoryService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = httpContextAccessor;
        }

        public async Task<List<CategoryListModel>> GetCategories()
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"/Category");

            return await response.ReadContentAs<List<CategoryListModel>>();
        }

        public async Task<(List<CategoryListModel> categories, Metadata pagination)> GetCategories(CategoryRequestFilter filters)
        {
            StringBuilder categoryFilters = new StringBuilder();
            categoryFilters.Append($"Page={filters.Page}&PageSize={filters.PageSize}");

            var response = await _client.GetAsync($"/Category?{categoryFilters}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            var pagination = JsonSerializer.Deserialize<Metadata>(response.Headers.FirstOrDefault(_ => _.Key == "X-Pagination").Value.FirstOrDefault());
            return (await response.Content.ReadFromJsonAsync<List<CategoryListModel>>(), pagination);
        }

        public async Task CreateCategory(CategoryCreateModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PostAsJsonAsync($"/Category", model);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);
        }

        public async Task<bool> UpdateCategory(CategoryUpdateModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJsonAsync($"/Category", model);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> DeleteCategory(string categoryId)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"/Category/{categoryId}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }

        private void AddAuthorizationHeader()
        {
            var claimsIdentity = _context.HttpContext.User.Identity as ClaimsIdentity;
            var bearerTokenClaim = claimsIdentity?.FindFirst("BearerToken");
            if (bearerTokenClaim != null)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenClaim.Value);
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
    }
}
