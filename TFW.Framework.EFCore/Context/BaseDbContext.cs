﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Cross.Models;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Context
{
    public abstract class BaseDbContext : DbContext, IFullAuditableDbContext
    {
        protected readonly QueryFilterOptions queryFilterOptions;

        public BaseDbContext()
        {
            queryFilterOptions = new QueryFilterOptions();
        }

        public BaseDbContext(QueryFilterOptions queryFilterOptions)
        {
            this.queryFilterOptions = queryFilterOptions ?? new QueryFilterOptions();
        }

        public BaseDbContext(DbContextOptions options,
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

        public virtual Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return this.BeginTransactionAsyncDefault(cancellationToken);
        }

        public virtual QueryFilter GetClonedFilter(string filterName)
        {
            return queryFilterOptions.FilterMap[filterName].Clone();
        }

        public virtual IHighLevelDbContext EnableFilter(params string[] filterNames)
        {
            queryFilterOptions.EnableFilter(filterNames);

            return this;
        }

        public virtual IHighLevelDbContext DisableFilter(params string[] filterNames)
        {
            queryFilterOptions.DisableFilter(filterNames);

            return this;
        }

        public virtual IHighLevelDbContext ReplaceOrAddFilter(params QueryFilter[] filters)
        {
            queryFilterOptions.ReplaceOrAddFilter(filters);

            return this;
        }

        public virtual bool IsFilterEnabled(string filterName)
        {
            return queryFilterOptions.IsEnabled(filterName);
        }

        public virtual bool IsFilterAppliedForEntity(string filterName, Type eType)
        {
            return queryFilterOptions.IsAppliedForEntity(filterName, eType);
        }

        public virtual IQueryable<T> QueryDeleted<T>() where T : class, ISoftDeleteEntity
        {
            return this.QueryDeletedDefault<T>();
        }

        public virtual Task<EntityEntry<E>> ReloadAsync<E>(E entity) where E : class
        {
            return this.ReloadAsyncDefault(entity);
        }

        public virtual EntityEntry<E> Remove<E>(E entity, bool isPhysical = false) where E : class
        {
            return this.RemoveDefault(entity, isPhysical);
        }

        public virtual void RemoveRange<E>(IEnumerable<E> list, bool isPhysical = false) where E : class
        {
            this.RemoveRangeDefault(list, isPhysical);
        }

        public virtual Task<EntityEntry<E>> RemoveAsync<E>(object[] key, bool isPhysical = false) where E : class
        {
            return this.RemoveAsyncDefault<E>(key, isPhysical);
        }

        public virtual Task<int> SqlRemoveAllAsync(string tblName)
        {
            return this.SqlRemoveAllAsyncDefault(tblName);
        }
    }
}