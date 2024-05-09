namespace ECommerce.UI.Models.Dtos.ViewModels.Baskets
{
    public class BasketCheckoutModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        // BillingAddress
        public AddressCheckoutDto Address { get; set; }
        // Payment
        public PaymentCardCheckoutDto PaymentCard { get; set; }
    }
}
