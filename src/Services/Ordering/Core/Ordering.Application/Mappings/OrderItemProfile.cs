using AutoMapper;
using Ordering.Application.Models.Dtos;
using Ordering.Application.Models.Dtos.OrderItems;
using Ordering.Domain.Entities;
using Shared.Models.Basket;

namespace Ordering.Application.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<ShoppingCartItemCheckoutDto, OrderItemAddDto>();
            CreateMap<OrderItemAddDto, OrderItem>();
            CreateMap<OrderItem, OrderItemListDto>();
        }
    }
}
