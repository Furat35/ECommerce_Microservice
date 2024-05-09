using ECommerce.UI.Extensions;
using ECommerce.UI.Helpers;
using ECommerce.UI.Helpers.Filters;
using ECommerce.UI.Models;
using ECommerce.UI.Models.Dtos.ViewModels.Products;
using ECommerce.UI.Services.Contracts;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ECommerce.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public CatalogService(HttpClient client, IHttpContextAccessor context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context;
        }

        public async Task<(IEnumerable<ProductListModel> products, Metadata pagination)> GetProducts(ProductRequestFilter filters)
        {
            StringBuilder productFilters = new StringBuilder();
            if (filters.Name != null)
                productFilters.Append($"Name={filters.Name}&");
            productFilters.Append($"Page={filters.Page}&PageSize={filters.PageSize}");

            var response = await _client.GetAsync($"/Catalog?{productFilters}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            var pagination = JsonSerializer.Deserialize<Metadata>(response.Headers.FirstOrDefault(_ => _.Key == "X-Pagination").Value.FirstOrDefault());
            return (await response.Content.ReadFromJsonAsync<List<ProductListModel>>(), pagination);
        }

        public async Task<ProductListModel> GetProductById(string id)
        {
            var response = await _client.GetAsync($"/Catalog/{id}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.Content.ReadFromJsonAsync<ProductListModel>();
        }

        public async Task<(ProductListWithCategoryModel products, Metadata pagination)> GetProductsByCategoryId(ProductRequestFilter filters)
        {
            StringBuilder productFilters = new StringBuilder();
            if (filters.Name != null)
                productFilters.Append($"Name={filters.Name}&");
            if (filters.CategoryId != null)
                productFilters.Append($"CategoryId={filters.CategoryId}&");
            productFilters.Append($"Page={filters.Page}&PageSize={filters.PageSize}");

            var response = await _client.GetAsync($"/Catalog/Category?{productFilters}");
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            var pagination = JsonSerializer.Deserialize<Metadata>(response.Headers.FirstOrDefault(_ => _.Key == "X-Pagination").Value.FirstOrDefault());
            return (await response.Content.ReadFromJsonAsync<ProductListWithCategoryModel>(), pagination);
        }

        public async Task<string> CreateProduct(ProductCreateModel model)
        {
            var formFile = _context.HttpContext.Request.Form.Files.FirstOrDefault();
            AddAuthorizationHeader();
            var response = await _client.PostAsJsonWithFormFile($"/Catalog", model, formFile);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> UpdateProduct(ProductUpdateModel model)
        {
            AddAuthorizationHeader();
            var response = await _client.PutAsJsonAsync($"/Catalog", model);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }
        public async Task<bool> UpdateProductPhoto(IFormFile file, string productId)
        {
            AddAuthorizationHeader();
            // Create multipart form data
            using var formData = new MultipartFormDataContent();

            // Convert IFormFile to StreamContent
            using var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);

            // Send PUT request
            var response = await _client.PutAsync($"/Catalog/{productId}", formData);
            await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            return await response.ReadContentAs<bool>();
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"/Catalog/{productId}");
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
