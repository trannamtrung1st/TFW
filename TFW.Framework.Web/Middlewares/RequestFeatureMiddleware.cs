using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Web.Models;

namespace TFW.Framework.Web.Middlewares
{
    public class RequestFeatureMiddleware : CalledOncePerScopedMiddeware
    {
        private SimpleHttpRequestFeature _originalRequest;

        protected override async Task InvokeCoreAsync(HttpContext context, RequestDelegate next)
        {
            if (_originalRequest == null)
            {
                _originalRequest = new SimpleHttpRequestFeature(context.Request);
                context.Features.Set(_originalRequest);
            }

            await next(context);
        }
    }
}
