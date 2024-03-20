namespace Authentication.API.Models.Dtos.PaymentCards
{
    public class PaymentCardListDto
    {
        public Guid Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public string PaymentMethod { get; set; }
    }
}
