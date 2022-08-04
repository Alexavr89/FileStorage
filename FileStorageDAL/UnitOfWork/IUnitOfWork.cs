using FileStorageDAL.Repository;
using System;
using System.Threading.Tasks;

namespace FileStorageDAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStorageFileRepository StorageFiles { get; }
        Task<int> SaveAsync();
    }
}
