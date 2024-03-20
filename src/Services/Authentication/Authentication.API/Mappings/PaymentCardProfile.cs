using Authentication.API.Entities;
using Authentication.API.Models.Dtos.PaymentCards;
using AutoMapper;

namespace Authentication.API.Mappings
{
    public class PaymentCardProfile : Profile
    {
        public PaymentCardProfile()
        {
            CreateMap<PaymentCardAddDto, PaymentCard>();
            CreateMap<PaymentCard, PaymentCardListDto>();
        }
    }
}
