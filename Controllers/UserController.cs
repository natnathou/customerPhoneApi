using System.Threading.Tasks;
using customerPhoneApi.Dtos;
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
                return Ok(_userService.GetAllUsers());
            }
            catch (System.Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        [HttpPost("new")]
        public async Task<IActionResult> Post(PostUserDto user)
        {
            try
            {
                return Ok(await _userService.AddUser(user));

            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                return Ok(await _userService.RemoveAllUsers());


            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }


    }
}