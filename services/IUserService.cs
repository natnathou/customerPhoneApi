using System.Collections.Generic;
using System.Threading.Tasks;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi.services
{
    public interface IUserService
    {
        ResponseService<List<UserDto>> GetAllUsers();
        Task<ResponseService<UserDto>> AddUser(UserDto user);
        Task<ResponseService<List<UserDto>>> RemoveAllUsers();
    }
}