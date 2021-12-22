using FileStorageDAL.Entities;
using FileStorageDAL.Models;
using FileStorageDAL.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<(IEnumerable<StorageFile> files, int count)> GetAllFilesAsync(StorageFilesRequest filesRequest)
        {
            var files = _context.StorageFiles
               .Include(file => file.ApplicationUser)
               .SearchBy(filesRequest.SearchTerm);

            var totalCount = await files.CountAsync();

            var resultfiles = await files
                .Sort(filesRequest.OrderBy)
                .PageStorageFiles(filesRequest.PageNumber, filesRequest.PageSize)
                .ToListAsync();

            return (resultfiles, totalCount);
        }

        public async Task<StorageFile> GetFileByFileIdAsync(Guid fileId)
        {
            return await _context.StorageFiles
                 .FirstOrDefaultAsync(
                     file => file.Id == fileId);
        }

        public async Task<StorageFile> GetPrivateFileByIdAsync(Guid fileId)
        {
            return await _context.StorageFiles
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && !file.IsRecycled);
        }

        public async Task<(IEnumerable<StorageFile> files, int count)> GetPrivateFilesByUserAsync(ApplicationUser user, StorageFilesRequest filesRequest)
        {
            var files = _context.StorageFiles
                .Where(file => file.ApplicationUser == user
                               && !file.IsRecycled)
                .FilterStorageFilesBySize(filesRequest.MinSize, filesRequest.MaxSize)
                .SearchBy(filesRequest.SearchTerm);

            var totalCount = await files.CountAsync();

            var resultCollection = await files
                .Sort(filesRequest.OrderBy)
                .PageStorageFiles(filesRequest.PageNumber, filesRequest.PageSize)
                .ToListAsync();

            return (resultCollection, totalCount);
        }

        public async Task<StorageFile> GetPublicFileByIdAsync(Guid fileId)
        {
            return await _context.StorageFiles
            .FirstOrDefaultAsync(
            file => file.Id == fileId
                && !file.IsRecycled && file.IsPublic);
        }

        public async Task<(IEnumerable<StorageFile> files, int count)> GetPublicFilesAsync(StorageFilesRequest filesRequest)
        {
            var files = _context.StorageFiles
                .Include(file => file.ApplicationUser)
                .Where(file => file.IsPublic && !file.IsRecycled)
                .SearchBy(filesRequest.SearchTerm);

            var totalCount = await files.CountAsync();

            var resultfiles = await files
                .Sort(filesRequest.OrderBy)
                .PageStorageFiles(filesRequest.PageNumber, filesRequest.PageSize)
                .ToListAsync();

            return (resultfiles, totalCount);
        }

        public async Task<StorageFile> GetRecycledFileByIdAsync(Guid fileId)
        {
            return await _context.StorageFiles
            .FirstOrDefaultAsync(
            file => file.Id == fileId
                && file.IsRecycled);
        }

        public async Task<(IEnumerable<StorageFile> files, int count)> GetRecycledFilesByUserAsync(ApplicationUser user, StorageFilesRequest filesRequest)
        {
            var files = _context.StorageFiles
            .Where(file => file.ApplicationUser == user
                   && file.IsRecycled)
            .SearchBy(filesRequest.SearchTerm);

            var totalCount = await files.CountAsync();

            var resultfiles = await files
                .Sort(filesRequest.OrderBy)
                .PageStorageFiles(filesRequest.PageNumber, filesRequest.PageSize)
                .ToListAsync();

            return (resultfiles, totalCount);
        }
    }
}
