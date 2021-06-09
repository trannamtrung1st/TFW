using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.UoW
{
    public interface IDbUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        DbContextId ContextId { get; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IQueryFilterUnitOfWork
    {
        QueryFilter GetClonedFilter(string filterName);

        IQueryFilterUnitOfWork EnableFilter(params string[] filterNames);

        IQueryFilterUnitOfWork DisableFilter(params string[] filterNames);

        IQueryFilterUnitOfWork ReplaceOrAddFilter(params QueryFilter[] filters);

        bool IsFilterEnabled(string filterName);

        bool IsFilterAppliedForEntity(string filterName, Type eType);

        bool IsFilterEnabledAndAppliedForEntity(string filterName, Type eType);
    }

    public interface IDisposableUnitOfWork : IDisposable, IAsyncDisposable
    {
    }

    public interface IBaseUnitOfWork : IDisposableUnitOfWork, IDbUnitOfWork, IQueryFilterUnitOfWork
    {
    }
}
