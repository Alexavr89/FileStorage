using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FileStorageDAL.Entities
{
    /// <summary>
    /// Extended class for IdentityUser with extra functionality
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public DateTime Created { get; set; }
        public virtual ICollection<StorageFile> StorageFiles { get; set; }
    }
}
