using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;

namespace Authentication.API.Models.Dtos.User
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public AddressListDto Address { get; set; }
        public PaymentCardListDto PaymentCard { get; set; }
    }
}
