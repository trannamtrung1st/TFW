using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.EFCore
{
    public interface IDbMigrator
    {
        void CreateOrMigrateDatabase<TDbContext>(TDbContext dbContext) where TDbContext : DbContext;
        void CreateOrMigrateDatabase<TDbContext>(TDbContext dbContext, Action<TDbContext> seedAction) where TDbContext : DbContext;
    }

    public class DefaultDbMigrator : IDbMigrator
    {
        public DefaultDbMigrator()
        {
        }

        public virtual void CreateOrMigrateDatabase<TDbContext>(TDbContext dbContext) where TDbContext : DbContext
        {
            CreateOrMigrateDatabase(dbContext, null);
        }

        public virtual void CreateOrMigrateDatabase<TDbContext>(
            TDbContext dbContext, Action<TDbContext> seedAction) where TDbContext : DbContext
        {
            dbContext.Database.Migrate();
            seedAction?.Invoke(dbContext);
        }
    }
}
