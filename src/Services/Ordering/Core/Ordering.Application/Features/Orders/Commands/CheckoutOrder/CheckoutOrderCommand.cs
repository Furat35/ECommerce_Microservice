using MediatR;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.PaymentCards;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<Guid>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid UserId { get; set; }
        // BillingAddress
        public AddressAddDto Address { get; set; }
        // Payment
        public PaymentCardAddDto PaymentCard { get; set; }
    }
}
