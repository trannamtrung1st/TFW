using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Context
{
    public abstract class BaseIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
        IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>,
        IFullAuditableDbContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        protected readonly QueryFilterOptions queryFilterOptions;
        public QueryFilterOptions QueryFilterOptions => queryFilterOptions;

        public BaseIdentityDbContext()
        {
            queryFilterOptions = new QueryFilterOptions();
        }

        public BaseIdentityDbContext(QueryFilterOptions queryFilterOptions)
        {
            this.queryFilterOptions = queryFilterOptions ?? new QueryFilterOptions();
        }

        public BaseIdentityDbContext(DbContextOptions options,
            IOptionsSnapshot<QueryFilterOptions> queryFilterOptions) : base(options)
        {
            this.queryFilterOptions = queryFilterOptions.Value;
        }

        public virtual void AuditEntities()
        {
            this.AuditEntitiesDefault();
        }

        public virtual bool IsSoftDeleteEnabled()
        {
            return this.IsSoftDeleteEnabledDefault();
        }

        public virtual bool IsSoftDeleteAppliedForEntity(Type eType)
        {
            return this.IsSoftDeleteAppliedForEntityDefault(eType);
        }

        public virtual void PrepareAdd(object entity)
        {
            this.PrepareAddDefault(entity);
        }

        public virtual void PrepareModify(object entity)
        {
            this.PrepareModifyDefault(entity);
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public virtual EntityEntry SoftDelete(object entity)
        {
            return this.SoftRemoveDefault(entity);
        }

        public virtual EntityEntry<T> SoftDelete<T>(T entity) where T : class
        {
            return this.SoftRemoveDefault(entity);
        }

        public virtual void SoftDeleteRange(params object[] entities)
        {
            foreach (var e in entities)
                SoftDelete(e);
        }

        public virtual bool TryAttach(object entity, out EntityEntry entry)
        {
            return this.TryAttachDefault(entity, out entry);
        }

        public virtual bool TryAttach<E>(E entity, out EntityEntry<E> entry) where E : class
        {
            return this.TryAttachDefault(entity, out entry);
        }

        public virtual EntityEntry<E> Update<E>(E entity, Action<E> patchAction)
            where E : class
        {
            return this.UpdateDefault(entity, patchAction);
        }

        public virtual EntityEntry<E> Update<E>(E entity, params Expression<Func<E, object>>[] changedProperties)
            where E : class
        {
            return this.UpdateDefault(entity, changedProperties);
        }

        public virtual Task<EntityEntry<E>> UpdateAsync<E>(E entity, Func<E, Task> patchAction)
            where E : class
        {
            return this.UpdateAsyncDefault(entity, patchAction);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return this.BeginTransactionAsyncDefault(cancellationToken);
        }
    }

    public abstract class BaseIdentityDbContext<TUser, TRole, TKey> : BaseIdentityDbContext<TUser, TRole, TKey,
        IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>,
        IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        protected BaseIdentityDbContext()
        {
        }

        protected BaseIdentityDbContext(QueryFilterOptions queryFilterOptions) : base(queryFilterOptions)
        {
        }

        protected BaseIdentityDbContext(DbContextOptions options, IOptionsSnapshot<QueryFilterOptions> queryFilterOptions) : base(options, queryFilterOptions)
        {
        }
    }

    public abstract class BaseIdentityDbContext<TUser> : BaseIdentityDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser<string>
    {
        protected BaseIdentityDbContext()
        {
        }

        protected BaseIdentityDbContext(QueryFilterOptions queryFilterOptions) : base(queryFilterOptions)
        {
        }

        protected BaseIdentityDbContext(DbContextOptions options, IOptionsSnapshot<QueryFilterOptions> queryFilterOptions) : base(options, queryFilterOptions)
        {
        }
    }

    public abstract class BaseIdentityDbContext : BaseIdentityDbContext<IdentityUser>
    {
        protected BaseIdentityDbContext()
        {
        }

        protected BaseIdentityDbContext(QueryFilterOptions queryFilterOptions) : base(queryFilterOptions)
        {
        }

        protected BaseIdentityDbContext(DbContextOptions options, IOptionsSnapshot<QueryFilterOptions> queryFilterOptions) : base(options, queryFilterOptions)
        {
        }
    }
}
