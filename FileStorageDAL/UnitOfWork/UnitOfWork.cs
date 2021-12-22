using FileStorageDAL.Repository;
using System;
using System.Threading.Tasks;

namespace FileStorageDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileStorageDbContext _context;
        private IStorageFileRepository _storageFileRepository;

        public UnitOfWork(FileStorageDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public IStorageFileRepository StorageFiles
        {
            get
            {
                if (_storageFileRepository == null)
                {
                    _storageFileRepository = new StorageFileRepository(_context);
                }
                return _storageFileRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}
