using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileStorageDAL
{
    public class FileStorageDbContext : IdentityDbContext<ApplicationUser>
    {
        public FileStorageDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<StorageFile> StorageFiles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StorageFile>()
                .HasOne(storageItem => storageItem.ApplicationUser)
                .WithMany(user => user.StorageFiles);
        }
    }
}
