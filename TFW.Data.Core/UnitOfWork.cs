using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.DI.Attributes;
using TFW.Framework.UoW;

namespace TFW.Data.Core
{
    [ScopedService(ServiceType = typeof(IUnitOfWork))]
    public class UnitOfWork : BaseUnitOfWork<DataContext>, IUnitOfWork
    {
        public UnitOfWork(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
