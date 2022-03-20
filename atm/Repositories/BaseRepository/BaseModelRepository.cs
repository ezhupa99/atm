using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using atm.Data;
using atm.Models;
using Microsoft.EntityFrameworkCore;

namespace atm.Repositories.BaseRepository
{
    public class BaseModelModelRepository<TEntity> : IBaseModelRepository<TEntity> where TEntity : class, IBaseModel
    {
        private DbSet<TEntity> DbSet { get; }

        private readonly ComprogContext _context;

        public BaseModelModelRepository(ComprogContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> GetByIdAsyncNonTracking(int id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.Where(ExcludeDeleted()).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsyncNonTracking()
        {
            return await DbSet.AsNoTracking().Where(ExcludeDeleted()).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void DeleteAsync(TEntity entity)
        {
            var dbEntityEntry = _context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            entity.Deleted = DateTime.UtcNow;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Modified;
                entity.Deleted = DateTime.UtcNow;
            }
        }

        public IQueryable<TEntity> CustomQuery()
        {
            return DbSet.Where(ExcludeDeleted());
        }

        public IQueryable<TEntity> CustomQueryNonTracking()
        {
            return DbSet.AsNoTracking().Where(ExcludeDeleted());
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return await DbSet.Where(ExcludeDeleted()).CountAsync();

            return await DbSet.Where(predicate).Where(ExcludeDeleted()).CountAsync();
        }

        public Task SaveChangesAsync()
        {
            SetBaseProperties();
            return _context.SaveChangesAsync(CancellationToken.None);
        }

        private void SetBaseProperties()
        {
            var entities = _context.ChangeTracker.Entries()
                .Where(x => x.Entity is IBaseModel && x.State is EntityState.Added or EntityState.Modified);

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        ((IBaseModel) entity.Entity).Created = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        ((IBaseModel) entity.Entity).Updated = DateTime.UtcNow;
                        break;
                }
            }
        }
        private static Expression<Func<TEntity, bool>> ExcludeDeleted()
        {
            return t => !t.Deleted.HasValue;
        }
    }
}