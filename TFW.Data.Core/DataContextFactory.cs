using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Data.Core
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {

        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable(DataConsts.ConnStrVarName));
            return new DataContextCore(optionsBuilder.Options);
        }
    }
}
