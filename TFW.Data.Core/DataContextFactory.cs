using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Data;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Factory;
using TFW.Framework.EFCore.Options;

namespace TFW.Data.Core
{
    [ScopedService(ServiceType = typeof(IDbContextFactory<DataContext>))]
    public class DataContextFactory : DbContextFactory<DataContext> //, IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IOptionsSnapshot<QueryFilterOptions> _queryFilters;
        private readonly IDbConnectionPoolManager _poolManager;

        public DataContextFactory(DbContextOptions<DataContext> options,
            IOptionsSnapshot<QueryFilterOptions> queryFilters,
            IDbConnectionPoolManager poolManager) : base(options)
        {
            _queryFilters = queryFilters;
            _poolManager = poolManager;
        }

        protected override DataContext CreateCore(DbContextOptions<DataContext> overrideOptions = null)
        {
            overrideOptions = overrideOptions ?? options;

            return new DataContext(overrideOptions, _queryFilters, _poolManager);
        }

        //public DataContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        //    optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrVarName));
        //    return new DataContext(optionsBuilder.Options);
        //}
    }
}
