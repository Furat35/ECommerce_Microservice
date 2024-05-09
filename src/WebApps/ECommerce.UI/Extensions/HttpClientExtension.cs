using System.Net.Http.Headers;
using System.Text.Json;

namespace ECommerce.UI.Extensions
{
    public static class HttpClientExtension
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static async Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await httpClient.PostAsync(url, content);
        }

        public static async Task<HttpResponseMessage> PostAsJsonWithFormFile<T>(this HttpClient httpClient, string url, T data, IFormFile formFile) where T : class
        {
            // Create multipart form data
            using var formData = new MultipartFormDataContent();
            if (data != null)
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(data)?.ToString();
                    if (value != null)
                        formData.Add(new StringContent(value), property.Name);
                }

            // Convert IFormFile to StreamContent
            using var fileContent = new StreamContent(formFile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
            formData.Add(fileContent, "file", formFile.FileName);

            // Send POST request
            return await httpClient.PostAsync(url, formData);
        }

        public static async Task<HttpResponseMessage> Put<T>(this HttpClient httpClient, string url, T data, IFormFile formFile) where T : class
        {
            // Create multipart form data
            using var formData = new MultipartFormDataContent();
            if (data != null)
                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(data)?.ToString();
                    if (value != null)
                        formData.Add(new StringContent(value), property.Name);
                }

            // Convert IFormFile to StreamContent
            using var fileContent = new StreamContent(formFile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
            formData.Add(fileContent, "file", formFile.FileName);

            // Send POST request
            return await httpClient.PostAsync(url, formData);
        }

        public static async Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return await httpClient.PutAsync(url, content);
        }
    }
}
