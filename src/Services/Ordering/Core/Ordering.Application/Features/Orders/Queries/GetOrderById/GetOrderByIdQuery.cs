using MediatR;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderListDto>
    {
        public string OrderId { get; set; }

        public GetOrderByIdQuery(string orderId)
        {
            OrderId = orderId;
        }
    }
}
