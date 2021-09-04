using TFW.Docs.Data;
using TFW.Framework.DI.Attributes;
using TFW.Framework.UoW;

namespace TFW.Docs.Business.Core
{
    [ScopedService(ServiceType = typeof(IUnitOfWork))]
    public class UnitOfWork : BaseUnitOfWork<DataContext>, IUnitOfWork
    {
        public UnitOfWork(DataContext dbContext) : base(dbContext)
        {
        }
    }
}