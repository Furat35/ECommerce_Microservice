using Catalog.API.Models.Categories;

namespace Catalog.API.Models.Products
{
    public class ProductListDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CategoryListDto Category { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountedPrice
        {
            get
            {
                return (Price > Discount)
                    ? Price - Discount
                    : 1;
            }
        }
    }
}
