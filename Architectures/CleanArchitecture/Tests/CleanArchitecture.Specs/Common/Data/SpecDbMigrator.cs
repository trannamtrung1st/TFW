using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Specs.Common.Data
{
    public class SpecDbMigrator : DbMigrator, ISpecDbMigrator
    {
        public SpecDbMigrator(DataContext dbContext) : base(dbContext)
        {
        }

        public async Task InitAsync(string dataSetKey)
        {
            var isFirstTime = !(await _dbContext.Database.GetAppliedMigrationsAsync()).Any();

            await _dbContext.Database.MigrateAsync();

            if (isFirstTime)
            {
                var dataSets = DataSets.Get(dataSetKey);

                await _dbContext.AddRangeAsync(dataSets.Customers);
                await _dbContext.AddRangeAsync(dataSets.Employees);
                await _dbContext.AddRangeAsync(dataSets.Products);
                await _dbContext.AddRangeAsync(dataSets.Sales);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
