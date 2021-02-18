using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
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
