using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.OrderItems;
using Ordering.Application.Models.Dtos.PaymentCards;

namespace Ordering.Application.Models.Dtos.Orders
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        // BillingAddress
        public AddressListDto Address { get; set; }
        // Payment
        public PaymentCardListDto PaymentCard { get; set; }
        // OrderItems
        public ICollection<OrderItemListDto> OrderItems { get; set; }

    }
}
