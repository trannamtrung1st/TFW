using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Providers
{
    public interface IRequestTimeZoneProvider
    {
        Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext);
    }
}
