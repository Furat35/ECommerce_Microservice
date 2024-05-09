using ECommerce.UI.Models.Dtos.ViewModels.Addresses;
using ECommerce.UI.Models.Dtos.ViewModels.OrderItems;
using ECommerce.UI.Models.Dtos.ViewModels.PaymentCards;

namespace ECommerce.UI.Models.Dtos.ViewModels.Orders
{
    public class OrderListModel
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        // BillingAddress
        public AddressListModel Address { get; set; }
        // Payment
        public PaymentCardListModel PaymentCard { get; set; }
        public ICollection<OrderItemListModel> OrderItems { get; set; }
    }
}
