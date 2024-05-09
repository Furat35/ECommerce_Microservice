using ECommerce.UI.Models.Dtos.ViewModels.Categories;

namespace ECommerce.UI.Models.Dtos.ViewModels.Products
{
    public class ProductListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CategoryListModel Category { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
