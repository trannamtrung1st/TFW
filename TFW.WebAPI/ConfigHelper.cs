using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using TFW.WebAPI.Middlewares;

namespace TFW.WebAPI
{
    public static class ConfigHelper
    {
        public static IApplicationBuilder UsePrincipalInfoMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PrincipalInfoMiddleware>();
        }
    }
}
