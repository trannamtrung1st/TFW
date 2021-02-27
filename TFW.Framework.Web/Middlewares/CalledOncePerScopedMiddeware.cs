using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Middlewares
{
    public abstract class CalledOncePerScopedMiddeware : IMiddleware
    {
        private bool _called;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!_called)
            {
                _called = true;
                await InvokeCoreAsync(context, next);
                return;
            }

            await next(context);
        }

        protected abstract Task InvokeCoreAsync(HttpContext context, RequestDelegate next);
    }
}
