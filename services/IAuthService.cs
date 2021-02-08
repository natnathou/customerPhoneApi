using System.Threading.Tasks;
using customerPhoneApi.Dtos;

namespace customerPhoneApi.services
{
    public interface IAuthService
    {
        Task<ResponseService<string>> Authenticate(UserLoginDto User);
    }
}