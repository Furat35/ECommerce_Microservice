using Catalog.API.Entities;

namespace Catalog.API.Services.Contracts
{
    public interface IProductPhotoService
    {
        bool RemoveProductPhoto(Product product);
        Task<string> UploadProductPhoto(Product product);
    }
}
