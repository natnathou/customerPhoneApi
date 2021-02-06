
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using customerPhoneApi.Dtos;
using customerPhoneApi.models;

namespace customerPhoneApi.services
{
  public class UserService : IUserService
  {


    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public ResponseService<List<GetUserDto>> GetUsers()
    {
      var response = new ResponseService<List<GetUserDto>>();
      User user;
      List<User> _users;
      try
      {
        user = new User
        {
          Name = "Patrick",
          Email = "patrick@gmail.com",
          PhoneNumber = "0122332322",
          Address = "22 rue emile zola, Monteux, France",
          DateCreation = DateTime.Now
        };

        _users = new List<User>(){
        new User{
            Name = "Patrick",
            Email = "patrick@gmail.com",
            PhoneNumber = "0122332jj322",
            Address = "22 rue emile zola, Monteux, France",
            DateCreation = DateTime.Now
            },
        new User{
            Name = "Alan",
            Email = "alan@gmail.com",
            PhoneNumber = "0144442322",
            Address = "22 rue emile zola, Nice, France",
            DateCreation = DateTime.Now
            },
        new User{
            Name = "Lisa",
            Email = "lisa@gmail.com",
            PhoneNumber = "0546722322",
            Address = "45 rue emile zola, Paris, France",
            DateCreation = DateTime.Now
            }
      };

        response.Data = _users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
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
  }
}