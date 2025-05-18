using Catalog.API.Entities;
using Catalog.API.Services.Contracts;

namespace Catalog.API.Services
{
    public class ProductPhotoService(IHttpContextAccessor httpContextAccessor, IFileService fileService) : IProductPhotoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IFileService _fileService = fileService;

        public bool RemoveProductPhoto(Product product)
        {
            if (product.ImageFile is null)
                return false;

            try
            {
                _fileService.RemoveFile(product.ImageFile);
            }
            catch
            {
                // log
            }
            product.ImageFile = null;

            return true;
        }


        public async Task<string> UploadProductPhoto(Product product)
        {
            var imageToUpload = _httpContextAccessor.HttpContext.Request.Form.Files.FirstOrDefault();
            if (imageToUpload != null)
            {
                try
                {
                    if (product.ImageFile != null)
                        _fileService.RemoveFile(product.ImageFile);
                }
                catch (Exception)
                {
                    // todo : log can be taken
                }

                string photoPath = await _fileService.UploadFile("productimages", imageToUpload);
                product.ImageFile = photoPath;

                return photoPath;
            }

            return null;
        }
    }
}
