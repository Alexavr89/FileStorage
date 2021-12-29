using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FileStorageDAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public virtual ICollection<StorageFile> StorageFiles { get; set; }
    }
}
