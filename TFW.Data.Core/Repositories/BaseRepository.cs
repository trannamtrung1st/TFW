using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFW.Cross;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Data.Repositories;

namespace TFW.Data.Core.Repositories
{
    public abstract partial class BaseRepository<E> : IBaseRepository<E> where E : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<E> dbSet;

        public BaseRepository(DbContext context)
        {
            this.dbContext = context;
            this.dbSet = context.Set<E>();
        }

        public virtual IQueryable<T> Limit<T>(IQueryable<T> query, int page, int pageLimit)
        {
            if (page <= 0 || pageLimit <= 0)
                throw AppException.From(Cross.ResultCode.InvalidPagingRequest);

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

        public virtual EntityEntry<E> Remove(E entity)
        {
            return dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<E> list)
        {
            dbSet.RemoveRange(list);
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
            EntityEntry<E> entry;

            TryAttach(entity, out entry);

            patchAction.Invoke(entity);

            return entry;
        }

        public virtual EntityEntry<E> Update(E entity, params Expression<Func<E, object>>[] changedProperties)
        {
            EntityEntry<E> entry;

            TryAttach(entity, out entry);

            if (changedProperties?.Any() == true)
            {
                foreach (var property in changedProperties)
                {
                    entry.Property(property).IsModified = true;
                }
            }
            else return Update(entity);

            return entry;
        }

        public virtual EntityEntry<E> Attach(E entity)
        {
            return dbSet.Attach(entity);
        }

        public virtual bool TryAttach(E entity, out EntityEntry<E> entry)
        {
            entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry = dbSet.Attach(entity);

                return true;
            }

            return false;
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

        public virtual async Task<E> FirstOrDefaultAsync()
        {
            return await dbSet.FirstOrDefaultAsync();
        }


        public virtual async Task<EntityEntry<E>> RemoveAsync(params object[] key)
        {
            var entity = await FindAsync(key);

            if (entity == null)
                throw AppException.From(Cross.ResultCode.EntityNotFound);

            return dbSet.Remove(entity);
        }

        public virtual async Task<E> FindAsync(params object[] key)
        {
            return await dbSet.FindAsync(key);
        }

        public virtual async Task<int> SqlRemoveAllAsync()
        {
            var result = await dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {EntityName}");
            
            return result;
        }

        /*
		********************* ABSTRACT AREA *********************
		*/

        public abstract string EntityName { get; }
    }
}
