using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {

        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            var orderList = (await GetAsync(o => o.UserId == Guid.Parse(userId), [_ => _.Address, _ => _.PaymentCard])).ToList();
            return orderList;
        }
    }
}
