using FileStorageBLL.Interfaces;
using FileStorageDAL.Entities;
using FileStorageDAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FileStorageBLL.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddFile(IFormFile uploadedFile)
        {
            _unitOfWork.StorageFiles.AddFile(uploadedFile);
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
    }
}
