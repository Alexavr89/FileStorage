using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace FileStorageDAL
{
    public static class FileStorageSeeder
    {
        static RoleManager<IdentityRole> roleManager;
        static UserManager<ApplicationUser> userManager;

        public static void SeedFiles(FileStorageDbContext context)
        {
            if (!context.StorageFiles.Any())
            {
                var files = new List<StorageFile>
                {
                    new StorageFile {Name = "text" },
                    new StorageFile {Name = "image" },

                };
                var roles = new List<IdentityRole>
                {
                    new IdentityRole{ Name = "Admin"},
                    new IdentityRole { Name = "User"}
                };
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser {UserName = "Admin"},
                    new ApplicationUser {UserName = "User"}
                };
                
                userManager.AddToRoleAsync(context.Users.First(x=>x.UserName == "Admin"),"Admin");
                userManager.AddToRoleAsync(context.Users.First(x=>x.UserName == "User"),"User");
                context.AddRange(files, roles, users);
                context.SaveChanges();
            }
        }
    }
}
