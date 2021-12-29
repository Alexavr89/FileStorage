using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace FileStorageDAL
{
    public class FileStorageSeeder
    {
        public static void SeedFiles(FileStorageDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!context.StorageFiles.Any())
            {
                var hasher = new PasswordHasher<ApplicationUser>();
                var file1 = new StorageFile
                {
                    IsPublic = true,
                    Created = DateTime.Now,
                    Name = "First file",
                    Extension = "txt",
                    Id = 1
                };
                var file2 = new StorageFile
                {
                    IsPublic = false,
                    Created = DateTime.Now,
                    Name = "Second file",
                    Extension = "pdf",
                    Id = 2
                };
                var files = new List<StorageFile>
                {
                    file1, file2
                };
                var user1 = new ApplicationUser
                {
                    UserName = "admin",
                    PasswordHash = hasher.HashPassword(null, "Admin"),
                    NormalizedUserName = "Admin",
                    Created = DateTime.Now,
                    StorageFiles = files,
                };
                var user2 = new ApplicationUser
                {
                    UserName = "user",
                    PasswordHash = hasher.HashPassword(null, "User"),
                    NormalizedUserName = "User",
                    Created = DateTime.Now,
                    StorageFiles = files,
                };
                var role1 = new IdentityRole
                {
                    Name = "Admin",
                };
                var role2 = new IdentityRole
                {
                    Name = "User"
                };

                var res = userManager.CreateAsync(user1, "$aquaFor123");
                if (res.Result.Succeeded)
                {
                    roleManager.CreateAsync(role1).Wait();
                }

                userManager.AddToRoleAsync(user1, "Admin").Wait();

                userManager.CreateAsync(user2, "#sometimeOne123").Wait();

                roleManager.CreateAsync(role2).Wait();

                userManager.AddToRoleAsync(user2, "User").Wait();
                context.SaveChangesAsync();
            }
        }
    }
}
