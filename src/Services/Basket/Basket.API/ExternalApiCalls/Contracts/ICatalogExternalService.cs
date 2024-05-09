using Basket.API.Models.ExternalApiResponseDtos;

namespace Basket.API.ExternalApiCalls.Contracts
{
    public interface ICatalogExternalService
    {
        Task<ProductListDto> GetProductById(string productId);
    }
}
