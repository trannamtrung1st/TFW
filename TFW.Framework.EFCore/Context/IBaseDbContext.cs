using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.EFCore.Context
{
    public interface IBaseDbContext :
        IDisposable, IAsyncDisposable,
        IInfrastructure<IServiceProvider>, IDbContextDependencies,
        IDbSetCache, IDbContextPoolable, IResettableService
    {
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }
        DbContextId ContextId { get; }
        EntityEntry Add([NotNullAttribute] object entity);
        EntityEntry<TEntity> Add<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        ValueTask<EntityEntry> AddAsync([NotNullAttribute] object entity, CancellationToken cancellationToken = default);
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([NotNullAttribute] TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        void AddRange([NotNullAttribute] params object[] entities);
        void AddRange([NotNullAttribute] IEnumerable<object> entities);
        Task AddRangeAsync([NotNullAttribute] IEnumerable<object> entities, CancellationToken cancellationToken = default);
        Task AddRangeAsync([NotNullAttribute] params object[] entities);
        EntityEntry Attach([NotNullAttribute] object entity);
        EntityEntry<TEntity> Attach<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        void AttachRange([NotNullAttribute] IEnumerable<object> entities);
        void AttachRange([NotNullAttribute] params object[] entities);
        EntityEntry Entry([NotNullAttribute] object entity);
        EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;
        object Find([NotNullAttribute] Type entityType, params object[] keyValues);
        ValueTask<object> FindAsync([NotNullAttribute] Type entityType, object[] keyValues, CancellationToken cancellationToken);
        ValueTask<object> FindAsync([NotNullAttribute] Type entityType, params object[] keyValues);
        ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
        ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
        EntityEntry Remove([NotNullAttribute] object entity);
        EntityEntry<TEntity> Remove<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        void RemoveRange([NotNullAttribute] IEnumerable<object> entities);
        void RemoveRange([NotNullAttribute] params object[] entities);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry Update([NotNullAttribute] object entity);
        EntityEntry<TEntity> Update<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        void UpdateRange([NotNullAttribute] IEnumerable<object> entities);
        void UpdateRange([NotNullAttribute] params object[] entities);
        bool TryAttach(object entity, out EntityEntry entry);
        bool TryAttach<E>(E entity, out EntityEntry<E> entry) where E : class;
        EntityEntry<E> Update<E>(E entity, Action<E> patchAction)
            where E : class;
        EntityEntry<E> Update<E>(E entity, params Expression<Func<E, object>>[] changedProperties)
            where E : class;
        Task<EntityEntry<E>> UpdateAsync<E>(E entity, Func<E, Task> patchAction)
            where E : class;
    }

}
