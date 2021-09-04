using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TFW.Docs.Cross.Providers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.EFCore.Factory;
using TFW.Framework.EFCore.Options;

namespace TFW.Docs.Data
{
    [ScopedService(ServiceType = typeof(IDbContextFactory<DataContext>))]
    public class DataContextFactory : DbContextFactory<DataContext> //, IDesignTimeDbContextFactory<DataContext>
    {
        private readonly IOptionsSnapshot<QueryFilterOptions> _queryFilters;
        private readonly IBusinessContextProvider _businessContextProvider;

        public DataContextFactory(DbContextOptions<DataContext> options,
            IOptionsSnapshot<QueryFilterOptions> queryFilters,
            IBusinessContextProvider businessContextProvider) : base(options)
        {
            _queryFilters = queryFilters;
            _businessContextProvider = businessContextProvider;
        }

        protected override DataContext CreateCore(DbContextOptions<DataContext> overrideOptions = null)
        {
            overrideOptions = overrideOptions ?? options;

            return new DataContext(overrideOptions, _queryFilters, _businessContextProvider);
        }

        //public DataContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        //    optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrKey));
        //    return new DataContext(optionsBuilder.Options);
        //}
    }
}
