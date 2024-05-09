namespace ECommerce.UI.Models.Dtos.ViewModels.Products
{
    public class ProductUpdateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public decimal Price { get; set; }
    }
}
