using AutoMapper;
using Ordering.Application.Models.Dtos.PaymentCards;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class PaymentCardProfile : Profile
    {
        public PaymentCardProfile()
        {
            CreateMap<PaymentCardAddDto, PaymentCard>();
            CreateMap<PaymentCardUpdateDto, PaymentCard>().ForMember(_ => _.Id, opt => opt.Ignore());
            CreateMap<PaymentCard, PaymentCardListDto>();
        }
    }
}
