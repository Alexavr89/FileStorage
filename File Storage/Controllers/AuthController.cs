using File_Storage.Models;
using FileStorageBLL.Account;
using FileStorageBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using WebApiDemo.Filters;
using WebApiDemo.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace File_Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [ApiController]
        [Route("[controller]")]
        [ModelStateActionFilter]
        public class AccountController : Controller
        {
            private readonly IUserService _userService;
            private readonly JwtSettings _jwtSettings;

            public AccountController(
                IUserService userService,
                IOptionsSnapshot<JwtSettings> jwtSettings)
            {
                _userService = userService;
                _jwtSettings = jwtSettings.Value;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterModel model)
            {
                await _userService.Register(new Register
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    Year = model.Year
                });

                return Created(string.Empty, string.Empty);
            }

            [HttpPost("logon")]
            public async Task<IActionResult> Logon(LogonModel model)
            {
                var user = await _userService.Logon(new Logon
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (user is null) return BadRequest();

                var roles = await _userService.GetRoles(user);

                return Ok(JwtHelper.GenerateJwt(user, roles, _jwtSettings));
            }
        }
    }
}
