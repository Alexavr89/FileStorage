using FileStorageDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorageDAL.Repository
{
    public interface IStorageFileRepository : IRepositoryBase<StorageFile>
    {
        IEnumerable<StorageFile> GetAllFiles(string query);
        IEnumerable<StorageFile> GetFilesByUser(string userId);
        void AddFile(IFormFile uploadedFile, string userName);
        void DeleteFile(int id);
        IEnumerable<StorageFile> GetPrivateFilesByUser(string userId);
        IEnumerable<StorageFile> GetPublicFilesByUser(string userId);
        Task SetFilePublic(int fileId, bool IsPublic);
        Task SetFilePrivate(int fileId, bool IsPrivate);
    }
}
