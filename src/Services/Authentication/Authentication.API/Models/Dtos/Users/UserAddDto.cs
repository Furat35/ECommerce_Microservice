using Authentication.API.Enums;
using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;

namespace Authentication.API.Models.Dtos.Users
{
    public class UserAddDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public AddressAddDto? Address { get; set; }
        public PaymentCardAddDto? PaymentCard { get; set; }
    }
}
