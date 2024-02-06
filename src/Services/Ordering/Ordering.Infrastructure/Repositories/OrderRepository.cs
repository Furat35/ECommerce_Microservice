using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {

        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            var orderList = await _dbContext.Orders
                                    .Where(o => o.UserName.ToLower() == username.ToLower())
                                    .ToListAsync();
            return orderList;
        }
    }
}
