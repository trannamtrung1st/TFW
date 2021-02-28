using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.Cross.Models;

namespace TFW.Framework.EFCore.Context
{
    public interface IFullAuditableDbContext : IBaseDbContext
    {
        // Should not be called directly
        void AuditEntities();
        void PrepareAdd(object entity);
        void PrepareModify(object entity);

        EntityEntry SoftDelete(object entity);
        EntityEntry<T> SoftDelete<T>(T entity) where T : class;
        void SoftDeleteRange(params object[] entities);
        bool IsSoftDeleteEnabled();
        bool IsSoftDeleteAppliedForEntity(Type eType);
        IQueryable<T> QueryDeleted<T>() where T : class, ISoftDeleteEntity;
        EntityEntry<E> Remove<E>(E entity, bool isPhysical = false) where E : class;
        void RemoveRange<E>(IEnumerable<E> list, bool isPhysical = false) where E : class;
        Task<EntityEntry<E>> RemoveAsync<E>(object[] key, bool isPhysical = false) where E : class;
    }
}
