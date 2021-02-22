using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.EFCore.Options;

namespace TFW.Framework.EFCore.Context
{
    public interface IHighLevelDbContext : IDisposable, IAsyncDisposable
    {
        QueryFilter GetClonedFilter(string filterName);
        IHighLevelDbContext EnableFilter(params string[] filterNames);
        IHighLevelDbContext DisableFilter(params string[] filterNames);
        IHighLevelDbContext ReplaceOrAddFilter(params QueryFilter[] filters);
        bool IsFilterEnabled(string filterName);
        bool IsFilterAppliedForEntity(string filterName, Type eType);
        bool IsFilterEnabledAndAppliedForEntity(string filterName, Type eType);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        DbContextId ContextId { get; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
