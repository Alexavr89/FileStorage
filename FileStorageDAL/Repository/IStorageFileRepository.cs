using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileStorageDAL.Repository
{
    public interface IStorageFileRepository : IRepositoryBase<StorageFile>
    {
        IEnumerable<StorageFile> GetAllFiles(string query);
        IEnumerable<StorageFile> GetFilesByUser(string userId);
        void AddFile(IFormFile uploadedFile, string userName);
        void DeleteFile(int id);
    }
}
