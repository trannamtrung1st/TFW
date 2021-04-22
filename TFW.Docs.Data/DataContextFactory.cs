using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Providers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Factory;
using TFW.Framework.EFCore.Options;

namespace TFW.Data.Core
{
    [ScopedService(ServiceType = typeof(IDbContextFactory<DataContext>))]
    public class DataContextFactory : DbContextFactory<DataContext> //, IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IOptionsSnapshot<QueryFilterOptions> _queryFilters;

        public DataContextFactory(DbContextOptions<DataContext> options,
            IOptionsSnapshot<QueryFilterOptions> queryFilters) : base(options)
        {
            _queryFilters = queryFilters;
        }

        [Inject(Required = false)]
        public IBusinessContextProvider BusinessContextProvider { get; set; }

        protected override DataContext CreateCore(DbContextOptions<DataContext> overrideOptions = null)
        {
            overrideOptions = overrideOptions ?? options;

            return new DataContext(overrideOptions, _queryFilters)
            {
                BusinessContextProvider = BusinessContextProvider
            };
        }

        //public DataContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        //    optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrKey));
        //    return new DataContext(optionsBuilder.Options);
        //}
    }
}
