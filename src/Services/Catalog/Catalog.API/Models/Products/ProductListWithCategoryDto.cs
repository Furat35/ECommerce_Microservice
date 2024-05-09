namespace Catalog.API.Models.Products
{
    public class ProductListWithCategoryDto
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductListDto> Products { get; set; }
    }
}
