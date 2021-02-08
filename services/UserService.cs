using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using customerPhoneApi.Data;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;
using customerPhoneApi.utility;
using Microsoft.AspNetCore.Http;

namespace customerPhoneApi.services
{
  public class UserService : IUserService
  {


    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
      _context = context;
      _mapper = mapper;
    }

    public ResponseService<List<GetUserDto>> GetAllUsers()
    {
      var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split("bearer ").Last();
      var currentUser = _context.Users.FirstOrDefault(u => u.Token == token);

      var response = new ResponseService<List<GetUserDto>>();

      try
      {


        response.Data = _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
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

    public async Task<ResponseService<GetUserDto>> AddUser(PostUserDto user)
    {

      var response = new ResponseService<GetUserDto>();
      try
      {
        new ObjectValidation().userIsValid(user);

        User userTrueFormat = _mapper.Map<User>(user);
        User userExist = _context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);

        if (userExist != null)
          throw new System.Exception("User Already exist");

        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
        userTrueFormat.PasswordHash = passwordHash;
        userTrueFormat.Salt = passwordSalt;

        await _context.Users.AddAsync(userTrueFormat);
        await _context.SaveChangesAsync();

        response.Data = _mapper.Map<GetUserDto>(user);
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

    public async Task<ResponseService<List<GetUserDto>>> RemoveAllUsers()
    {
      var response = new ResponseService<List<GetUserDto>>();

      try
      {
        _context.RemoveRange(_context.Users);
        await _context.SaveChangesAsync();
        response.Data = _context.Users.Select((u) => _mapper.Map<GetUserDto>(u)).ToList();
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


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }

  }
}