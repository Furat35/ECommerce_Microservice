using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Filters;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Domain.Entities;
using Shared.Helpers;

namespace Ordering.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderRepository(OrderContext orderContext, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(orderContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<List<OrderListDto>> GetOrdersByUserId(string userId, OrderRequestFilter filters)
        {
            var orders = (await GetAsync(o => o.UserId == Guid.Parse(userId), [_ => _.Address, _ => _.PaymentCard, _ => _.OrderItems])).AsQueryable();

            var filteredProducts = new OrderFilterService(_mapper, orders).FilterOrders(filters);
            new HeaderService(_httpContextAccessor).AddToHeaders(filteredProducts.Headers);

            return filteredProducts.ResponseValue;
        }
    }
}
