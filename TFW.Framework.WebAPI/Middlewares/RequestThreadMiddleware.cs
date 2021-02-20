using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.WebAPI.Middlewares
{
    public class RequestThreadMiddleware : CalledOncePerScopedMiddeware
    {
        protected override async Task InvokeCoreAsync(HttpContext context, RequestDelegate next)
        {
            RequestThread.HttpContext = context;

            await next(context);
        }
    }
}
