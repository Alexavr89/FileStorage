using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace File_Storage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            if (user == null)
                return BadRequest("Please use a valid user");
            await _userManager.CreateAsync(user);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return (IActionResult)_userManager.Users;
        }

        [HttpGet("{id}")]
        public Task<ApplicationUser> GetUser(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        [HttpPut]
        public async Task<IActionResult> EditUser(string id, [FromBody] ApplicationUser newuser)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("There is no user with such Id");
            }
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction("GetUser", new { id = newuser.Id }, newuser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("There is no user with such Id");
            }
            await _userManager.DeleteAsync(user);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("GetUsers");
        }
    }
}
