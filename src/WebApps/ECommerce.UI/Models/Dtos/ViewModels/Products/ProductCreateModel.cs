namespace ECommerce.UI.Models.Dtos.ViewModels.Products
{
    public class ProductCreateModel
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
