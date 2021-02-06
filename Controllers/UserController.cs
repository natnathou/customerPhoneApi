using customerPhoneApi.services;
using Microsoft.AspNetCore.Mvc;

namespace customerPhoneApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    public IActionResult Get()
    {
      try
      {
        return Ok(_userService.GetUsers());
      }
      catch (System.Exception ex)
      {
        return Ok(ex.Message);
      }


    }


  }
}