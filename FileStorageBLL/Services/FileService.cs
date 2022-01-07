using FileStorageBLL.Interfaces;
using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorageBLL.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddFile(IFormFile uploadedFile, string userName)
        {
            _unitOfWork.StorageFiles.AddFile(uploadedFile, userName);
        }

        public void DeleteFile(int id)
        {
            _unitOfWork.StorageFiles.DeleteFile(id);
        }

        public IEnumerable<StorageFile> GetAllFiles(string query)
        {
            return _unitOfWork.StorageFiles.GetAllFiles(query);
        }

        public IEnumerable<StorageFile> GetFilesByUser(string userId)
        {
            return _unitOfWork.StorageFiles.GetFilesByUser(userId);
        }

        public IEnumerable<StorageFile> GetPrivateFilesByUser(string userId)
        {
            return _unitOfWork.StorageFiles.GetPrivateFilesByUser(userId);
        }

        public IEnumerable<StorageFile> GetPublicFilesByUser(string userId)
        {
            return _unitOfWork.StorageFiles.GetPublicFilesByUser(userId);
        }

        public async Task SetFilePrivate(int fileId)
        {
            await _unitOfWork.StorageFiles.SetFilePrivate(fileId);
        }

        public async Task SetFilePublic(int fileId)
        {
            await _unitOfWork.StorageFiles.SetFilePublic(fileId);
        }
    }
}
