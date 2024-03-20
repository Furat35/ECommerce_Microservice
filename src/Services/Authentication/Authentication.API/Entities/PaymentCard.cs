using Authentication.API.Common;
using Authentication.API.Enums;

namespace Authentication.API.Entities
{
    public class PaymentCard : EntityBase
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public User User { get; set; }
    }
}
