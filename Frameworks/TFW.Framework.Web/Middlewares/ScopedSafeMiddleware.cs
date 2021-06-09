using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Middlewares
{
    public abstract class ScopedSafeMiddleware : IMiddleware
    {
        private bool _hasException;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await SafeCallAsync(ForwardInvokeAsync, context);

            await next(context);

            await SafeCallAsync(BackwardInvokeAsync, context);
        }

        protected virtual async Task SafeCallAsync(Func<HttpContext, Task> func, HttpContext context)
        {
            if (!_hasException)
            {
                try
                {
                    await func(context);
                }
                catch (Exception e)
                {
                    _hasException = true;
                    throw e;
                }
            }
        }

        protected abstract Task ForwardInvokeAsync(HttpContext context);
        protected abstract Task BackwardInvokeAsync(HttpContext context);
    }
}
