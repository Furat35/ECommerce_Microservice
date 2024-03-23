using AutoMapper;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Domain.Entities;
using Shared.Models.Addresses;

namespace Ordering.Application.Mappings
{
    internal class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressAddDto, Address>();
            CreateMap<AddressUpdateDto, Address>().ForMember(_ => _.Id, opt => opt.Ignore());
            CreateMap<Address, AddressListDto>();
            CreateMap<AddressCheckoutDto, AddressAddDto>();
        }
    }
}
