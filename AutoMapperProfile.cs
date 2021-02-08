using AutoMapper;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PostUserDto, User>();
            CreateMap<User, GetUserDto>();
            CreateMap<PostUserDto, GetUserDto>();
        }
    }
}