using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TFW.Framework.Web.Features;

namespace TFW.Framework.Web.Middlewares
{
    public class RequestFeatureMiddleware : CalledOncePerScopedMiddeware
    {
        private ISimpleHttpRequestFeature _originalRequest;

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
