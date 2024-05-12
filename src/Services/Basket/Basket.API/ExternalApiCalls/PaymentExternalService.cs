using Basket.API.ExternalApiCalls.Contracts;
using System.Text.Json;

namespace Basket.API.ExternalApiCalls
{
    public class PaymentExternalService : IPaymentExternalService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;

        public PaymentExternalService(HttpClient client, IHttpContextAccessor context)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _context = context;
        }

        public async Task<bool> ProcessPayment()
        {
            var response = await _client.PostAsync($"api/v1/Payments", null);
            //await ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(response);

            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<bool>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //private async Task ThrowHttpRequestExceptionIfHttpRequestIsNotSuccessfull(HttpResponseMessage response)
        //{
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        string errorContent = await response.Content.ReadAsStringAsync();
        //        var errorDetails = JsonSerializer.Deserialize<ErrorDetail>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //        throw new HttpRequestException(message: errorDetails.ErrorMessage, null, statusCode: (HttpStatusCode)(errorDetails.StatusCode));
        //    }
        //}
    }
}
