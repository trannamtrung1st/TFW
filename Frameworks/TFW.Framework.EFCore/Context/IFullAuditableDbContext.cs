using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Cross.Audit;

namespace TFW.Framework.EFCore.Context
{
    public interface IFullAuditableDbContext : IBaseDbContext
    {
        // Should not be called directly
        void AuditEntities();
        void PrepareAdd(object entity);
        void PrepareModify(object entity);

        EntityEntry SoftDelete(object entity);
        EntityEntry<T> SoftDelete<T>(T entity) where T : class, ISoftDeleteEntity;
        void SoftDeleteRange(params object[] entities);
        bool IsSoftDeleteEnabled();
        bool IsSoftDeleteAppliedForEntity(Type eType);
        IQueryable<T> QueryDeleted<T>() where T : class, ISoftDeleteEntity;
        EntityEntry<E> Remove<E>(E entity, bool isPhysical) where E : class, ISoftDeleteEntity;
        void RemoveRange<E>(IEnumerable<E> list, bool isPhysical) where E : class, ISoftDeleteEntity;
        Task<EntityEntry<E>> RemoveAsync<E>(object[] key, bool isPhysical) where E : class, ISoftDeleteEntity;
    }
}
