namespace Catalog.API.Services.Contracts
{
    public interface IFileService
    {
        Task<string> UploadFile(string folderNameToUpload, IFormFile file);
        void RemoveFile(string filePath);
        string GetImage(string filePath);
    }
}
