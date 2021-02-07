using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using customerPhoneApi.Data;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;
using customerPhoneApi.utility;

namespace customerPhoneApi.services
{
    public class UserService : IUserService
    {


        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public ResponseService<List<UserDto>> GetAllUsers()
        {
            var response = new ResponseService<List<UserDto>>();

            try
            {


                response.Data = _context.Users.Select(u => _mapper.Map<UserDto>(u)).ToList();
                response.Success = true;
                response.Message = "Response was sent correctly to the client.";

                return response;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            response.Success = true;
            response.Message = "Response was sent correctly to the client.";

            return response;
        }

        public async Task<ResponseService<UserDto>> AddUser(UserDto user)
        {

            var response = new ResponseService<UserDto>();
            try
            {
                new ObjectValidation().userIsValid(user);
                User userTrueFormat = _mapper.Map<User>(user);
                await _context.Users.AddAsync(userTrueFormat);
                await _context.SaveChangesAsync();
                response.Data = user;
                response.Success = true;
                response.Message = "User creation successfull!";
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ResponseService<List<UserDto>>> RemoveAllUsers()
        {
            var response = new ResponseService<List<UserDto>>();

            try
            {
                _context.RemoveRange(_context.Users);
                await _context.SaveChangesAsync();
                response.Data = _context.Users.Select((u) => _mapper.Map<UserDto>(u)).ToList();
                response.Success = true;
                response.Message = "Table was deleted!";

            }
            catch (System.Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}