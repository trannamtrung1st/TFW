using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TFW.Framework.EFCore.Factory
{
    public interface IDbContextFactory<TDbContext> : IDisposable, IAsyncDisposable where TDbContext : DbContext
    {
        TDbContext Create(DbContextOptions<TDbContext> options = null);
    }

    public abstract class DbContextFactory<TDbContext> : IDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        protected readonly IList<TDbContext> dbContexts;
        protected readonly DbContextOptions<TDbContext> options;
        private bool disposedValue;

        public DbContextFactory(DbContextOptions<TDbContext> options)
        {
            this.options = options;
            this.dbContexts = new List<TDbContext>();
        }

        public virtual TDbContext Create(DbContextOptions<TDbContext> options = null)
        {
            var context = CreateCore(options);

            dbContexts.Add(context);

            return context;
        }

        protected abstract TDbContext CreateCore(DbContextOptions<TDbContext> options = null);

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    var tasks = new List<Task>();

                    // TODO: dispose managed state (managed objects)
                    foreach (var context in dbContexts)
                        tasks.Add(context.DisposeAsync().AsTask());

                    await Task.WhenAll(tasks);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DbContextFactory()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            DisposeAsync(disposing: true).Wait();
            GC.SuppressFinalize(this);
        }
    }
}