using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorageBLL.Interfaces
{
    public interface IFileService
    {
        IEnumerable<StorageFile> GetAllFiles(string query);
        IEnumerable<StorageFile> GetFilesByUser(string userId);
        void DeleteFile(int id);
        void AddFile(IFormFile uploadedFile, string userName);
        IEnumerable<StorageFile> GetPrivateFilesByUser(string userId);
        IEnumerable<StorageFile> GetPublicFilesByUser(string userId);
        Task SetFilePublic(int fileId, bool IsPublic);
        Task SetFilePrivate(int fileId, bool IsPrivate);
    }
}
