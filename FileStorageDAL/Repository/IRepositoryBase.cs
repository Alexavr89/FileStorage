using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorageDAL.Repository
{
    public interface IRepositoryBase<in TEntity>
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
    }
}
