using System.Linq;
using System.Threading.Tasks;

namespace FileStorageDAL.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly FileStorageDbContext _context;
        public RepositoryBase(FileStorageDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> FindAll()
        {
            return _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.FindAsync<TEntity>(id);
        }
        public void Update(TEntity entity)
        {
            var element = _context.StorageFiles.Find(entity);
            if (element != null)
            {
                _context.Entry(element).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }
    }
}
