using Basket.API.Models.ExternalApiResponseDtos;

namespace Basket.API.ExternalApiCalls.Contracts
{
    public interface ICatalogExternalService
    {
        Task<Product> GetProductById(string productId);
    }
}
