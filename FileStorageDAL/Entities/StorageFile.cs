using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FileStorageDAL.Entities
{
    /// <summary>
    /// StorageFile entity
    /// </summary>
    public class StorageFile
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        public long Size { get; set; }
        [MaxLength(60)]
        public string Extension { get; set; }
        public DateTime Created { get; set; }
        [Required]
        [MaxLength(900)]
        public string RelativePath { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [DefaultValue(false)]
        public bool IsRecycled { get; set; }
        [DefaultValue(false)]
        public bool IsPublic { get; set; }
    }
}

