using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileStorageBLL.Interfaces
{
    public interface IFileService
    {
        IEnumerable<StorageFile> GetAllFiles(string query);
        IEnumerable<StorageFile> GetFilesByUser(string userId);
        void DeleteFile(int id);
        void AddFile(IFormFile uploadedFile);
    }
}
