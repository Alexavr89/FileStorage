using System.Linq;
using System.Linq.Dynamic.Core;

namespace FileStorageDAL.Repository.Extensions
{
    public static class RepositoryStorageFileExtensions
    {
        public static IQueryable<StorageFile> PageStorageFiles(
           this IQueryable<StorageFile> StorageFiles, int pageNumber, int pageSize)
        {
            return StorageFiles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<StorageFile> FilterStorageFilesBySize(
            this IQueryable<StorageFile> StorageFiles, long minSize, long maxSize)
        {
            return StorageFiles.Where(StorageFile =>
                StorageFile.Size >= minSize
                && StorageFile.Size <= maxSize);
        }

        public static IQueryable<StorageFile> SearchBy(
            this IQueryable<StorageFile> StorageFiles, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return StorageFiles;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return StorageFiles.Where(StorageFile =>
                StorageFile.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<StorageFile> Sort(
            this IQueryable<StorageFile> StorageFiles, string orderByString)
        {
            if (string.IsNullOrWhiteSpace(orderByString))
                return StorageFiles.OrderBy(StorageFile => StorageFile.Name);

            string orderQuery = orderByString.CreateOrderQuery<StorageFile>();

            if (string.IsNullOrWhiteSpace(orderQuery))
                return StorageFiles.OrderBy(StorageFile => StorageFile.Name);

            return StorageFiles.OrderBy(orderQuery);
        }
    }
}
