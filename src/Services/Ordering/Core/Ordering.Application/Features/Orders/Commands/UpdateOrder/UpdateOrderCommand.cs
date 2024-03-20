using MediatR;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.PaymentCards;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        // BillingAddress
        public AddressUpdateDto Address { get; set; }
        // Payment
        public PaymentCardUpdateDto PaymentCard { get; set; }
    }
}
