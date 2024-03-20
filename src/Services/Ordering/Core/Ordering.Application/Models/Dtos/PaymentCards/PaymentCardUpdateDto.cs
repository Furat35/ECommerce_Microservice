namespace Ordering.Application.Models.Dtos.PaymentCards
{
    public class PaymentCardUpdateDto
    {
        public Guid Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
