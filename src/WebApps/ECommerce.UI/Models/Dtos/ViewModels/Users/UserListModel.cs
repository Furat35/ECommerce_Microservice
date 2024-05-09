using ECommerce.UI.Models.Dtos.ViewModels.Addresses;
using ECommerce.UI.Models.Dtos.ViewModels.PaymentCards;

namespace ECommerce.UI.Models.Dtos.ViewModels.Users
{
    public class UserListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public AddressListModel Address { get; set; }
        public PaymentCardListModel PaymentCard { get; set; }
    }
}
