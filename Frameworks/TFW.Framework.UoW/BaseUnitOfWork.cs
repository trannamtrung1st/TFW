using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Common.Extensions;
using TFW.Framework.EFCore.Context;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.UoW
{
    public abstract class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork where TDbContext : IHighLevelDbContext
    {
        protected readonly TDbContext dbContext;
        protected bool disposedValue;

        protected BaseUnitOfWork(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbContextId ContextId => dbContext.ContextId;

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.BeginTransactionAsync(cancellationToken);
        }

        public IQueryFilterUnitOfWork DisableFilter(params string[] filterNames)
        {
            dbContext.DisableFilter(filterNames);

            return this;
        }

        public IQueryFilterUnitOfWork EnableFilter(params string[] filterNames)
        {
            dbContext.EnableFilter(filterNames);

            return this;
        }

        public QueryFilter GetClonedFilter(string filterName)
        {
            return dbContext.GetClonedFilter(filterName);
        }

        public bool IsFilterAppliedForEntity(string filterName, Type eType)
        {
            return dbContext.IsFilterAppliedForEntity(filterName, eType);
        }

        public bool IsFilterEnabled(string filterName)
        {
            return dbContext.IsFilterEnabled(filterName);
        }

        public bool IsFilterEnabledAndAppliedForEntity(string filterName, Type eType)
        {
            return dbContext.IsFilterAppliedForEntity(filterName, eType);
        }

        public IQueryFilterUnitOfWork ReplaceOrAddFilter(params QueryFilter[] filters)
        {
            dbContext.ReplaceOrAddFilter(filters);

            return this;
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return dbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        #region Dispose
        protected virtual async Task DisposeManagedStateAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task DisposeUnmanagedStateAsync()
        {
            if (dbContext != null)
            {
                Func<ValueTask> dbContextDispose = dbContext.DisposeAsync;
                await (ValueTask)dbContextDispose.SafeCall().Item1;
            }
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    await DisposeManagedStateAsync();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                await DisposeUnmanagedStateAsync();
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~BaseUnitOfWork()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            DisposeAsync(disposing: false).Wait();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            DisposeAsync(disposing: true).Wait();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            await DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
