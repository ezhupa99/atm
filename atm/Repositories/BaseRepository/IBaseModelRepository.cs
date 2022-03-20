using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace atm.Repositories.BaseRepository
{
    public interface IBaseModelRepository<TEntity>
    {
        public Task<TEntity> GetByIdAsync(int id);

        public Task<TEntity> GetByIdAsyncNonTracking(int id);

        public Task<IEnumerable<TEntity>> GetAllAsync();

        public Task<IEnumerable<TEntity>> GetAllAsyncNonTracking();

        public Task AddAsync(TEntity entity);

        public void DeleteAsync(TEntity entity);

        public Task DeleteAsync(int id);

        public Task SaveChangesAsync();

        public IQueryable<TEntity> CustomQuery();
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);

        public IQueryable<TEntity> CustomQueryNonTracking();
    }
}