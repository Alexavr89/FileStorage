using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace File_Storage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private static RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/>.
        /// </summary>
        /// <param name="roleManager"></param>
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all roles from the database
        /// </summary>
        /// <returns>All roles from the database</returns>
        [HttpGet]
        public IEnumerable<IdentityRole> ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return roles;
        }

        /// <summary>
        /// Deletes speecified role from the Database
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Deletion confirmation message</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            try
            {
                if (role == null)
                {
                    return NotFound();
                }
                else
                {
                    var result = await _roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("ListRoles", new { id, saveChangesError = true });
            }
            return NoContent();
        }
    }
}
