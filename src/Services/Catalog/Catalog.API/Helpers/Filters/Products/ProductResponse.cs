using Shared.Helpers;

namespace Catalog.API.Helpers.Filters.Products
{
    public class ProductResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
