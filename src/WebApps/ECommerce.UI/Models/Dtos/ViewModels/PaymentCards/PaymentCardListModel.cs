using Shared.Enums;

namespace ECommerce.UI.Models.Dtos.ViewModels.PaymentCards
{
    public class PaymentCardListModel
    {
        public Guid Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
