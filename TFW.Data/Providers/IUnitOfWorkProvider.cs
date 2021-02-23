using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Data.Providers
{
    public interface IUnitOfWorkProvider
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
