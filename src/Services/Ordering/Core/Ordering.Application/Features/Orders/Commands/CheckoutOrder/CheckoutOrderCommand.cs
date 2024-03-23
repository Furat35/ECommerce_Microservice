using MediatR;
using Ordering.Application.Models.Dtos;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.PaymentCards;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<Guid>
    {
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public Guid UserId { get; set; }
        // BillingAddress
        public AddressAddDto Address { get; set; }
        // Payment
        public PaymentCardAddDto PaymentCard { get; set; }
        // OrderItems
        public List<OrderItemAddDto> OrderItems { get; set; }

    }
}
