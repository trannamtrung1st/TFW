using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.EFCore.Context;

namespace TFW.Framework.EFCore.Repository
{
    public partial interface IBaseRepository<E> where E : class
    {
        bool IsFilterAppliedForEntity(string filterName);
        bool IsSoftDeleteFilterAppliedForEntity();
        IQueryable<T> Limit<T>(IQueryable<T> query, int page, int pageLimit);
        Task<EntityEntry<E>> ReloadAsync(E entity);
        EntityEntry<E> Add(E entity);
        void AddRange(IEnumerable<E> entities);
        EntityEntry<E> Update(E entity);
        EntityEntry<E> Update(E entity, Action<E> patchAction);
        Task<EntityEntry<E>> UpdateAsync(E entity, Func<E, Task> patchAction);
        EntityEntry<E> Update(E entity, params Expression<Func<E, object>>[] changedProperties);
        void UpdateRange(IEnumerable<E> entities);
        EntityEntry<E> Attach(E entity);
        bool TryAttach(E entity, out EntityEntry<E> entry);
        EntityEntry<E> Remove(E entity, bool isPhysical = false);
        Task<EntityEntry<E>> RemoveAsync(object[] key, bool isPhysical = false);
        void RemoveRange(IEnumerable<E> list, bool isPhysical = false);
        Task<int> SqlRemoveAllAsync();
        ValueTask<E> FindAsync(params object[] key);
        IQueryable<E> AsTracking();
        IQueryable<E> AsNoTracking();
        IQueryable<E> Get();
        Task<E> FirstOrDefaultAsync();
        string EntityTableName { get; }
    }

    public abstract partial class BaseRepository<E, DbContextType> : IBaseRepository<E>
        where E : class
        where DbContextType : DbContext, IFullAuditableDbContext
    {
        protected readonly DbContextType dbContext;
        protected readonly DbSet<E> dbSet;

        public BaseRepository(DbContextType context)
        {
            this.dbContext = context;
            this.dbSet = context.Set<E>();
        }

        public bool IsFilterAppliedForEntity(string filterName)
        {
            return dbContext.IsFilterAppliedForEntity(filterName, typeof(E));
        }

        public bool IsSoftDeleteFilterAppliedForEntity()
        {
            return dbContext.IsSoftDeleteAppliedForEntity(typeof(E));
        }

        public virtual IQueryable<T> Limit<T>(IQueryable<T> query, int page, int pageLimit)
        {
            if (page <= 0 || pageLimit <= 0)
                throw new InvalidOperationException("Invalid paging request");

            query = query.Skip((page - 1) * pageLimit).Take(pageLimit);

            return query;
        }

        public virtual async Task<EntityEntry<E>> ReloadAsync(E entity)
        {
            var entry = dbContext.Entry(entity);

            await entry.ReloadAsync();

            return entry;
        }

        public virtual EntityEntry<E> Add(E entity)
        {
            return dbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<E> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual EntityEntry<E> Remove(E entity, bool isPhysical = false)
        {
            if (isPhysical)
                return dbSet.Remove(entity);

            return dbContext.SoftDelete(entity);
        }

        public virtual void RemoveRange(IEnumerable<E> list, bool isPhysical = false)
        {
            if (isPhysical)
                dbSet.RemoveRange(list);

            dbContext.SoftDeleteRange(list);
        }

        public virtual async Task<EntityEntry<E>> RemoveAsync(object[] key, bool isPhysical = false)
        {
            var entity = await FindAsync(key);

            if (entity == null) return null;

            if (isPhysical)
                return dbSet.Remove(entity);

            return dbContext.SoftDelete(entity);
        }

        public virtual EntityEntry<E> Update(E entity)
        {
            return dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<E> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public virtual EntityEntry<E> Update(E entity, Action<E> patchAction)
        {
            return dbContext.Update(entity, patchAction);
        }

        public virtual Task<EntityEntry<E>> UpdateAsync(E entity, Func<E, Task> patchAction)
        {
            return dbContext.UpdateAsync(entity, patchAction);
        }

        public virtual EntityEntry<E> Update(E entity, params Expression<Func<E, object>>[] changedProperties)
        {
            return dbContext.Update(entity, changedProperties);
        }

        public virtual EntityEntry<E> Attach(E entity)
        {
            return dbSet.Attach(entity);
        }

        public virtual bool TryAttach(E entity, out EntityEntry<E> entry)
        {
            return dbContext.TryAttach(entity, out entry);
        }

        public virtual IQueryable<E> AsNoTracking()
        {
            return dbSet.AsNoTracking();
        }

        public virtual IQueryable<E> AsTracking()
        {
            return dbSet.AsTracking();
        }

        public virtual IQueryable<E> Get()
        {
            return dbSet;
        }

        public virtual Task<E> FirstOrDefaultAsync()
        {
            return dbSet.FirstOrDefaultAsync();
        }

        public virtual ValueTask<E> FindAsync(params object[] key)
        {
            return dbSet.FindAsync(key);
        }

        public virtual Task<int> SqlRemoveAllAsync()
        {
            return dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {EntityTableName}");
        }

        /*
		********************* ABSTRACT AREA *********************
		*/

        public abstract string EntityTableName { get; }
    }
}
