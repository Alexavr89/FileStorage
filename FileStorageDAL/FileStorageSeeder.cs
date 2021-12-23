using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorageDAL
{
    public static class FileStorageSeeder
    {
        public static void SeedFiles(FileStorageDbContext context)
        {
            if (!context.StorageFiles.Any())
            {
                var files = new List<StorageFile>
                {
                    new StorageFile {Name = "text" },
                    new StorageFile {Name = "image" },

                };
                context.AddRange(files);
                context.SaveChanges();
            }
        }
    }
}
