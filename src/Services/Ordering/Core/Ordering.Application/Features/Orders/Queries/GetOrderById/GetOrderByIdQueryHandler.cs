using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderListDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderListDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetByIdAsync(Guid.Parse(request.OrderId), [_ => _.Address, _ => _.PaymentCard, _ => _.OrderItems]);
            return _mapper.Map<OrderListDto>(orderList);
        }
    }
}
