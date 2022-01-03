using FileStorageDAL.Entities;
using FileStorageDAL.Models;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace File_Storage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/>.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="roleManager"></param>
        public UsersController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves users with roles from the database
        /// </summary>
        /// <returns>Returns list of users with roles</returns>
        [HttpGet]
        public IEnumerable<UserRole> GetUsers()
        {
            try
            {
                var roles = _roleManager.Roles;
                var users = _userManager.Users;
                var userroles = new List<UserRole>();
                foreach (var user in users)
                {
                    foreach (var role in roles)
                    {
                        if (_userManager.IsInRoleAsync(user, role.Name).Result)
                        {
                            var ur = new UserRole()
                            {
                                UserId = user.Id,
                                RoleName = role.Name,
                                UserName = user.UserName
                            };
                            userroles.Add(ur);
                        }
                    }
                }
                return userroles;
            }
            catch (System.Exception e)
            {
                return (IEnumerable<UserRole>)NotFound(e.Message);
            }
        }

        /// <summary>
        /// Retrieves user from the database by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Retrieves user data</returns>
        [HttpGet("{id}")]
        public object GetUser(string id)
        {
            try
            {
                var user = _userManager.FindByIdAsync(id).Result;
                var role = _userManager.GetRolesAsync(user).Result;
                return new { user, role };
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Edit specifid user in role
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="role">User role</param>
        /// <param name="newuser">New user data</param>
        /// <returns>Returns updated user</returns>
        [HttpPut("{id}/{role}")]
        public async Task<IActionResult> EditUser(string id, string role, [FromBody] ApplicationUser newuser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("There is no user with such Id");
                }
                user.Email = newuser.Email;
                user.UserName = newuser.UserName;
                await _userManager.UpdateAsync(user);
                await _userManager.RemoveFromRoleAsync(user, _userManager.GetRolesAsync(user).Result[0]);
                await _userManager.AddToRoleAsync(user, role);
                await _unitOfWork.SaveAsync();
                return CreatedAtAction("GetUser", new { id = newuser.Id }, newuser);
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Delete user by specified id
        /// </summary>
        /// <param name="id">Id of specified user</param>
        /// <returns>Returns confirmation message about sucessful user deletion</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("There is no user with such Id");
                }
                await _userManager.DeleteAsync(user);
                await _unitOfWork.SaveAsync();
                return NoContent();
            }
            catch (System.Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
