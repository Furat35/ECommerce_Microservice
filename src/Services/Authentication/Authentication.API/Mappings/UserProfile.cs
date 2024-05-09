using Authentication.API.Entities;
using Authentication.API.Models.Dtos.User;
using Authentication.API.Models.Dtos.Users;
using AutoMapper;

namespace Authentication.API.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto, User>();
            CreateMap<User, UserListDto>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
