using MediatR;
using Ordering.Application.Filters;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderListDto>>
    {
        public string UserId { get; set; }
        public OrderRequestFilter Filters { get; set; }

        public GetOrdersListQuery(string userId, OrderRequestFilter filters)
        {
            UserId = userId;
            Filters = filters;
        }
    }
}
