using System.Collections.Generic;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi.services
{
  public interface IUserService
  {
    ResponseService<List<GetUserDto>> GetUsers();
    // User AddUser();
  }
}