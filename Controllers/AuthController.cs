using System.Threading.Tasks;
using customerPhoneApi.Dtos;
using customerPhoneApi.services;
using Microsoft.AspNetCore.Mvc;

namespace customerPhoneApi.Controllers
{

  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto User)
    {

      try
      {
        return Ok(await _authService.Authenticate(User));
      }
      catch
      {
        return BadRequest();
      }
    }

  }
}