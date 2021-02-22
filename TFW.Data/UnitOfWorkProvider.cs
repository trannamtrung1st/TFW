using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace TFW.Data
{
    public static class UnitOfWorkProvider
    {
        public static IAppUnitOfWork Current => HttpContext.GetRequiredService<IAppUnitOfWork>();
    }
}
