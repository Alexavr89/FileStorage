using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageDAL.Repository
{
    public class StorageFileRepository : RepositoryBase<StorageFile>, IStorageFileRepository
    {
        private readonly new FileStorageDbContext _context;
        public StorageFileRepository(FileStorageDbContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteFile(int id)
        {
            var file = _context.StorageFiles.FirstOrDefault(x => x.Id == id);
            if (file != null)
            {
                _context.StorageFiles.Remove(file);
                File.Delete(file.RelativePath);
            }
            _context.SaveChanges();
        }

        public IEnumerable<StorageFile> GetAllFiles(string query)
        {
            var files = _context.StorageFiles;
            if (query == null)
            {
                return files;
            }
            return files.Where(x => x.Name.ToLower().Contains(query.ToLower()));
        }

        public IEnumerable<StorageFile> GetFilesByUser(string userId)
        {
            return _context.StorageFiles.Where(x => x.ApplicationUser.Id == userId);
        }

        public void AddFile(IFormFile uploadedFile, string userName)
        {
            var user = _context.Users.Where(x => x.UserName == userName).FirstOrDefault();
            if (uploadedFile != null)
            {
                string path = "../FileStorageDAL/Files/" + uploadedFile.FileName;
                StorageFile file = new()
                {
                    Name = uploadedFile.FileName,
                    RelativePath = path,
                    Created = DateTime.Now,
                    Size = uploadedFile.Length,
                    Extension = uploadedFile.ContentType,
                    ApplicationUser = user,
                    IsPublic = true,
                    Id = _context.StorageFiles.Count() + 1
                };
                _context.StorageFiles.AddAsync(file);
                _context.SaveChangesAsync().Wait();
            }
        }

        public IEnumerable<StorageFile> GetPrivateFilesByUser(string userId)
        {
            return _context.StorageFiles.Where(file => !file.IsPublic && file.ApplicationUser.Id == userId);
        }

        public IEnumerable<StorageFile> GetPublicFilesByUser(string userId)
        {
            return _context.StorageFiles.Where(file => file.IsPublic && file.ApplicationUser.Id == userId);
        }

        public async Task SetFilePublic(int fileId)
        {
            var file = _context.StorageFiles.Where(file => file.Id == fileId).FirstOrDefault();
            file.IsPublic = true;
            await _context.SaveChangesAsync();
        }

        public async Task SetFilePrivate(int fileId)
        {
            var file = _context.StorageFiles.Where(file => file.Id == fileId).FirstOrDefault();
            file.IsPublic = false;
            await _context.SaveChangesAsync();
        }
    }
}
