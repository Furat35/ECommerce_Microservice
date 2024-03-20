using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.PaymentCards;
using Ordering.Domain.Entities;

namespace Ordering.Application.Models.Dtos.Orders
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        // BillingAddress
        public AddressListDto Address { get; set; }
        // Payment
        public PaymentCardListDto PaymentCard { get; set; }
    }
}
