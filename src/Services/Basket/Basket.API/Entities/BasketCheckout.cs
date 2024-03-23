using Basket.API.Models.Dtos.Addresses;
using Basket.API.Models.Dtos.PaymentCards;

namespace Basket.API.Entities
{
    public class BasketCheckout
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        // BillingAddress
        public AddressCheckoutDto Address { get; set; }
        // Payment
        public PaymentCardCheckoutDto PaymentCard { get; set; }
    }
}
