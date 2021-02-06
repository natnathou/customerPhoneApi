using AutoMapper;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<User, GetUserDto>();
    }
  }
}