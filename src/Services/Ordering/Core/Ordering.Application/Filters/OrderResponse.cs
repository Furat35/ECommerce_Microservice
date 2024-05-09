using Shared.Helpers;

namespace Ordering.Application.Filters
{
    public class OrderResponse<T> : ResponseFilter<T> where T : class, new()
    {
    }
}
