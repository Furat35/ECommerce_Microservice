namespace Ordering.Application.Models.Dtos.Addresses
{
    public class AddressUpdateDto
    {
        public Guid Id { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
