using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class Address : EntityBase
    {
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Order Order { get; set; }
    }
}
