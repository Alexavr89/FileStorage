using FileStorageDAL.Entities;
using FileStorageDAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorageDAL.Repository
{
    public interface IStorageFileRepository : IRepositoryBase<StorageFile>
    {
        Task<(IEnumerable<StorageFile> files, int count)> GetPrivateFilesByUserAsync(ApplicationUser user, StorageFilesRequest filesRequest);

        Task<StorageFile> GetPrivateFileByIdAsync(int fileId);

        Task<(IEnumerable<StorageFile> files, int count)> GetRecycledFilesByUserAsync(ApplicationUser user, StorageFilesRequest filesRequest);

        Task<StorageFile> GetRecycledFileByIdAsync(int fileId);

        Task<(IEnumerable<StorageFile> files, int count)> GetPublicFilesAsync(StorageFilesRequest filesRequest);

        Task<StorageFile> GetPublicFileByIdAsync(int fileId);

        Task<(IEnumerable<StorageFile> files, int count)> GetAllFilesAsync(StorageFilesRequest filesRequest);

        Task<StorageFile> GetFileByFileIdAsync(int fileId);
    }
}
