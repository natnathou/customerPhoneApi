using System.Collections.Generic;
using System.Threading.Tasks;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi.services
{
    public interface IUserService
    {
        ResponseService<List<GetUserDto>> GetAllUsers();
        Task<ResponseService<GetUserDto>> AddUser(PostUserDto user);
        Task<ResponseService<List<GetUserDto>>> RemoveAllUsers();
    }
}