using File_Storage.Models;
using FileStorageBLL.Account;
using FileStorageBLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiDemo.Filters;
using WebApiDemo.Helpers;

namespace File_Storage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/>.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="jwtSettings"></param>
        public AuthController(IUserService userService, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model"> New user data</param>
        /// <returns>Confirmation message about successful user registration</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await _userService.Register(new Register
                {
                    Email = model.Email,
                    Password = model.Password,
                    Year = model.Year
                });
                return Created(string.Empty, string.Empty);
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Logon method for registered user
        /// </summary>
        /// <param name="model">Logon credentials for user</param>
        /// <returns>Sucessful logon message</returns>
        [HttpPost("logon")]
        public async Task<IActionResult> Logon([FromBody] LogonModel model)
        {
            try
            {
                var user = await _userService.Logon(new Logon
                {
                    Email = model.Email,
                    Password = model.Password
                });

                if (user is null) return BadRequest();

                var roles = await _userService.GetRoles(user);

                return Ok(new
                {
                    token = JwtHelper.GenerateJwt(user, roles, _jwtSettings),
                    user,
                    roles
                });
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
