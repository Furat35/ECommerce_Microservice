using Authentication.API.Common;
using Authentication.API.Enums;

namespace Authentication.API.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public Address? Address { get; set; }
        public PaymentCard? PaymentCard { get; set; }
    }
}
