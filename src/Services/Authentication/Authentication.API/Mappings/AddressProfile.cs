using Authentication.API.Entities;
using Authentication.API.Models.Dtos.Addresses;
using AutoMapper;

namespace Authentication.API.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressAddDto, Address>();
            CreateMap<Address, AddressListDto>();
        }
    }
}
