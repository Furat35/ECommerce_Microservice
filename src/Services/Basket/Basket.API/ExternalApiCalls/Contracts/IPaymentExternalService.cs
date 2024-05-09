namespace Basket.API.ExternalApiCalls.Contracts
{
    public interface IPaymentExternalService
    {
        Task<bool> ProcessPayment();
    }
}
