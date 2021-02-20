using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.WebAPI.Middlewares
{
    public abstract class ScopedSafeMiddleware : IMiddleware
    {
        private bool _hasException;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_hasException)
            {
                await next(context);
                return;
            }
            try
            {
                await InvokeCoreAsync(context, next);
            }
            catch (Exception e)
            {
                _hasException = true;
                throw e;
            }
        }

        protected abstract Task InvokeCoreAsync(HttpContext context, RequestDelegate next);
    }
}
