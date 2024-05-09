namespace ECommerce.UI.Models.Dtos.ViewModels.Addresses
{
    public class AddressListModel
    {
        public Guid Id { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
