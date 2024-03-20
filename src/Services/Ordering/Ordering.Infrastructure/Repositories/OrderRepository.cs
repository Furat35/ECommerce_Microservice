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

        public async Task<List<Order>> GetOrdersByUsername(string username)
        {
            var orderList = (await GetAsync(o => o.UserName.ToLower() == username.ToLower(), [_ => _.Address, _ => _.PaymentCard])).ToList();
            return orderList;
        }
    }
}
