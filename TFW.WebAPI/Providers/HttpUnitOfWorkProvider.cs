using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFW.Data;
using TFW.Data.Providers;

namespace TFW.WebAPI.Providers
{
    public class HttpUnitOfWorkProvider : IUnitOfWorkProvider
    {
        public IUnitOfWork UnitOfWork => HttpContext.Current.RequestServices.GetRequiredService<IUnitOfWork>();
    }

}
