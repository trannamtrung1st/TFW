using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.DI;
using TFW.Framework.UoW;

namespace TFW.Data.Core
{
    [ScopedService(ServiceType = typeof(IAppUnitOfWork))]
    public class AppUnitOfWork : BaseUnitOfWork<DataContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
