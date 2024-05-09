namespace ECommerce.UI.Models.Dtos.ViewModels.Products
{
    public class ProductListWithCategoryModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductListModel> Products { get; set; }
    }
}
