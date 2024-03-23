
using Shared.Models.Addresses;
using Shared.Models.Basket;
using Shared.Models.PaymentCards;

namespace EventBus.Message.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }

        // BillingAddress
        public AddressCheckoutDto Address { get; set; }
        // Payment
        public PaymentCardCheckoutDto PaymentCard { get; set; }
        // Shopping Cart
        public List<ShoppingCartItemCheckoutDto> OrderItems { get; set; }
    }
}
