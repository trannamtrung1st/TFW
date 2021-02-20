using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.EFCore.Context
{
    public interface IFullAuditableDbContext : IBaseDbContext
    {
        void AuditEntities();
        void PrepareAdd(object entity);
        void PrepareModify(object entity);
        EntityEntry SoftDelete(object entity);
        EntityEntry<T> SoftDelete<T>(T entity) where T : class;
        void SoftDeleteRange(params object[] entities);
    }
}
