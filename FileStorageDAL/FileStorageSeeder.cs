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
        static UserManager<ApplicationUser> userManager;
        public static void SeedFiles(FileStorageDbContext context)
        {
            if (!context.StorageFiles.Any())
            {
                var files = new List<StorageFile>
                {
                    new StorageFile {Name = "text", IsPublic = true, ApplicationUser = context.Users.First(x=>x.UserName == "User")},
                    new StorageFile {Name = "image", IsPublic = false, ApplicationUser = context.Users.First(x=>x.UserName == "User") },

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
