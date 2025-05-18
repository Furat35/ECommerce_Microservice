using Basket.API.ExternalApiCalls.Contracts;
using Basket.API.Models.ExternalApiResponseDtos;

namespace Basket.API.ExternalApiCalls
{
    public class CatalogExternalService(IHttpClientFactory httpClientFactory) : ICatalogExternalService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Catalog.Api");

        public async Task<ProductListDto> GetProductById(string productId)
        {
            ProductListDto product = null;
            try
            {
                product = await _httpClient.GetFromJsonAsync<ProductListDto>($"api/v1/catalogs/{productId}");
            }
            catch (Exception ex)
            {
                //throw new HttpRequestException(ex.Message);
                //loging can be implemented
            }

            return product;
        }
    }
}
