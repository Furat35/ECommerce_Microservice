using Shared.Helpers;

namespace Catalog.API.Helpers.Filters.Products
{
    public class ProductRequestFilter : Pagination
    {
        public string Name { get; set; }
        public bool DateDesc { get; set; }
        public string CategoryId { get; set; }
    }
}
