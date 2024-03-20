using Authentication.API.Enums;

namespace Authentication.API.Models.Dtos.PaymentCards
{
    public class PaymentCardAddDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
