using Ordering.Application.Filters;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<List<OrderListDto>> GetOrdersByUserId(string userId, OrderRequestFilter filters);
    }
}
