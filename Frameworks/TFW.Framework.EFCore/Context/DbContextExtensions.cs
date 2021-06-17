using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Cross.Audit;
using TFW.Framework.EFCore.Query;

namespace TFW.Framework.EFCore.Context
{
    internal static class DbContextExtensions
    {
        public static IQueryable<T> QueryDeletedDefault<T>(this IFullAuditableDbContext dbContext) where T : class, ISoftDeleteEntity
        {
            var eType = typeof(T);

            if (dbContext.IsSoftDeleteAppliedForEntity(eType))
            {
                var clonedFilter = dbContext.GetClonedFilter(QueryFilterConsts.SoftDeleteDefaultName);

                var oldFilter = clonedFilter.ApplyFilter;
                clonedFilter.ApplyFilter = o => oldFilter(o) && o != eType;

                dbContext.ReplaceOrAddFilter(clonedFilter);
            }

            return dbContext.Set<T>().IsDeleted();
        }

        public static async Task<EntityEntry<E>> ReloadAsyncDefault<E>(this IFullAuditableDbContext dbContext, E entity) where E : class
        {
            var entry = dbContext.Entry(entity);

            await entry.ReloadAsync();

            return entry;
        }

        public static EntityEntry<E> RemoveDefault<E>(this IFullAuditableDbContext dbContext, E entity, bool isPhysical) where E : class
        {
            if (isPhysical)
                return dbContext.Remove(entity);

            return dbContext.SoftDelete(entity);
        }

        public static void RemoveRangeDefault<E>(this IFullAuditableDbContext dbContext, IEnumerable<E> list, bool isPhysical) where E : class
        {
            if (isPhysical)
            {
                dbContext.RemoveRange(list);
                return;
            }

            dbContext.SoftDeleteRange(list);
        }

        public static async Task<EntityEntry<E>> RemoveAsyncDefault<E>(this IFullAuditableDbContext dbContext, object[] key, bool isPhysical) where E : class
        {
            var entity = await dbContext.FindAsync<E>(key);

            if (entity == null)
                throw new KeyNotFoundException();

            if (isPhysical)
                return dbContext.Remove(entity);

            return dbContext.SoftDelete(entity);
        }

        public static Task<int> SqlRemoveAllAsyncDefault(this IFullAuditableDbContext dbContext, string tblName)
        {
            return dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {tblName}");
        }

        public static void AuditEntitiesDefault(this IFullAuditableDbContext dbContext)
        {
            var hasChanges = dbContext.ChangeTracker.HasChanges();
            if (!hasChanges) return;

            var entries = dbContext.ChangeTracker.Entries()
                .Where(o => o.State == EntityState.Modified ||
                    o.State == EntityState.Added).ToArray();

            foreach (var entry in entries)
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        dbContext.PrepareModify(entity);
                        break;
                    case EntityState.Added:
                        dbContext.PrepareAdd(entity);
                        break;
                }
            }
        }

        public static Task<IDbContextTransaction> BeginTransactionAsyncDefault(this IBaseDbContext dbContext,
            CancellationToken cancellationToken = default)
        {
            return dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public static bool IsSoftDeleteEnabledDefault(this IBaseDbContext dbContext)
        {
            return dbContext.IsFilterEnabled(QueryFilterConsts.SoftDeleteDefaultName);
        }

        public static bool IsSoftDeleteAppliedForEntityDefault(this IBaseDbContext dbContext, Type eType)
        {
            return typeof(ISoftDeleteEntity).IsAssignableFrom(eType)
                && dbContext.IsFilterAppliedForEntity(QueryFilterConsts.SoftDeleteDefaultName, eType);
        }

        public static void PrepareAddDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedTime = DateTimeOffset.UtcNow;
            }
        }

        public static void PrepareModifyDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            var isSoftDeleted = false;

            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                if (softDeleteEntity.IsDeleted)
                {
                    if (softDeleteEntity.DeletedTime != null)
                        throw new InvalidOperationException($"{nameof(entity)} is already deleted");

                    softDeleteEntity.DeletedTime = DateTimeOffset.UtcNow;
                    isSoftDeleted = true;
                }
            }

            if (!isSoftDeleted && entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.LastModifiedTime = DateTimeOffset.UtcNow;
            }
        }

        public static EntityEntry SoftDeleteDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;

                var entry = dbContext.Entry(entity);
                entry.Property(nameof(softDeleteEntity.IsDeleted)).IsModified = true;

                return entry;
            }

            throw new InvalidOperationException($"{nameof(entity)} is not {nameof(ISoftDeleteEntity)}");
        }

        public static EntityEntry<T> SoftDeleteDefault<T>(this IFullAuditableDbContext dbContext, T entity)
            where T : class
        {
            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;

                var entry = dbContext.Entry(entity);
                entry.Property(nameof(softDeleteEntity.IsDeleted)).IsModified = true;

                return entry;
            }

            throw new InvalidOperationException($"{nameof(entity)} is not {nameof(ISoftDeleteEntity)}");
        }

        public static bool TryAttachDefault<T>(this IFullAuditableDbContext dbContext, T entity, out EntityEntry<T> entry)
            where T : class
        {
            entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry = dbContext.Attach(entity);

                return true;
            }

            return false;
        }

        public static bool TryAttachDefault(this IFullAuditableDbContext dbContext, object entity, out EntityEntry entry)
        {
            entry = dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry = dbContext.Attach(entity);

                return true;
            }

            return false;
        }

        public static EntityEntry<E> UpdateDefault<E>(this IBaseDbContext dbContext, E entity, Action<E> patchAction)
            where E : class
        {
            EntityEntry<E> entry;

            dbContext.TryAttach(entity, out entry);

            patchAction(entity);

            return entry;
        }

        public static EntityEntry<E> UpdateDefault<E>(this IBaseDbContext dbContext,
            E entity, params Expression<Func<E, object>>[] changedProperties)
            where E : class
        {
            EntityEntry<E> entry;

            dbContext.TryAttach(entity, out entry);

            if (changedProperties?.Any() == true)
            {
                foreach (var property in changedProperties)
                    entry.Property(property).IsModified = true;
            }
            else return dbContext.Update(entity);

            return entry;
        }

        public static async Task<EntityEntry<E>> UpdateAsyncDefault<E>(this IBaseDbContext dbContext,
            E entity, Func<E, Task> patchAction)
            where E : class
        {
            EntityEntry<E> entry;

            dbContext.TryAttach(entity, out entry);

            await patchAction(entity);

            return entry;
        }
    }
}
