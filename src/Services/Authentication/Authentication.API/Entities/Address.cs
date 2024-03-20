using Authentication.API.Common;

namespace Authentication.API.Entities
{
    public class Address : EntityBase
    {
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public User User { get; set; }
    }
}
