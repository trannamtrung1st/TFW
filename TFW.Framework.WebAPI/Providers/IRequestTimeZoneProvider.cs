using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.WebAPI.Providers
{
    public interface IRequestTimeZoneProvider
    {
        Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext);
    }
}
