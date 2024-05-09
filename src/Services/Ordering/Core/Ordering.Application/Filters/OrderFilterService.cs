using AutoMapper;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Domain.Entities;
using Shared.Helpers;

namespace Ordering.Application.Filters
{
    public class OrderFilterService
    {
        private readonly IMapper _mapper;
        private IQueryable<Order> _order;

        public OrderFilterService(IMapper mapper, IQueryable<Order> orders)
        {
            _mapper = mapper;
            _order = orders;
        }

        public OrderResponse<List<OrderListDto>> FilterOrders(OrderRequestFilter filters)
        {
            int pageNumber = _order.Count() % filters.PageSize == 0 ? _order.Count() / filters.PageSize : _order.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _order.Count(), pageNumber);
            _order = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);
            var mappedOrders = _mapper.Map<List<OrderListDto>>(_order);

            return new()
            {
                ResponseValue = mappedOrders,
                Headers = header
            };
        }

        private IQueryable<Order> AddPagination(OrderRequestFilter filters)
          => _order
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize);
    }
}
