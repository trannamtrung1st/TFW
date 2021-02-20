using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.Cross.Models;
using TFW.Framework.i18n;

namespace TFW.Framework.EFCore.Context
{
    internal static class DbContextHelper
    {
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

        public static void PrepareAddDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            if (entity is IAuditableEntity == false) return;

            var auditableEntity = entity as IAuditableEntity;
            auditableEntity.CreatedTime = Time.Now;
        }

        public static void PrepareModifyDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            if (entity is IAuditableEntity == false) return;

            var auditableEntity = entity as IAuditableEntity;
            var isDeleted = true;
            if (auditableEntity is ISoftDeleteEntity)
            {
                var softDeleteEntity = auditableEntity as ISoftDeleteEntity;
                if (softDeleteEntity.IsDeleted)
                {
                    if (softDeleteEntity.DeletedTime != null)
                        throw new InvalidOperationException($"{nameof(entity)} is already deleted");

                    softDeleteEntity.DeletedTime = Time.Now;
                    isDeleted = true;
                }
            }

            if (!isDeleted) auditableEntity.LastModifiedTime = Time.Now;
        }

        public static EntityEntry SoftRemoveDefault(this IFullAuditableDbContext dbContext, object entity)
        {
            if (entity is ISoftDeleteEntity == false)
                throw new InvalidOperationException($"{nameof(entity)} is not {nameof(ISoftDeleteEntity)}");

            var softDeleteEntity = entity as ISoftDeleteEntity;
            softDeleteEntity.IsDeleted = true;

            var entry = dbContext.Entry(entity);
            entry.State = EntityState.Modified;

            return entry;
        }

        public static EntityEntry<T> SoftRemoveDefault<T>(this IFullAuditableDbContext dbContext, T entity)
            where T : class
        {
            if (entity is ISoftDeleteEntity == false)
                throw new InvalidOperationException($"{nameof(entity)} is not {nameof(ISoftDeleteEntity)}");

            var softDeleteEntity = entity as ISoftDeleteEntity;
            softDeleteEntity.IsDeleted = true;

            var entry = dbContext.Entry(entity);
            entry.State = EntityState.Modified;

            return entry;
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
