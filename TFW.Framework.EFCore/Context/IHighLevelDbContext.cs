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
    public interface IHighLevelDbContext
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        DbContextId ContextId { get; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        QueryFilterOptions QueryFilterOptions { get; }
    }
}
