using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.WebAPI.Middlewares;

namespace TFW.WebAPI.Extensions
{
    public static class ConfigExtensions
    {
        public static IApplicationBuilder UsePrincipalInfoMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PrincipalInfoMiddleware>();
        }
    }
}
