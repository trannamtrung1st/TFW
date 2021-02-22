using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross.Helpers;
using TFW.Framework.DI;

namespace TFW.WebAPI.Middlewares
{
    [ScopedService]
    public class RequestDataExtractionMiddleware : IMiddleware
    {
        private readonly IIdentityService _identityService;

        public RequestDataExtractionMiddleware(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var principal = context.User;
            var principalInfo = _identityService.MapToPrincipalInfo(principal);

            context.SetPrincipalInfo(principalInfo);

            await next(context);
        }
    }
}
