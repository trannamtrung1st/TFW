using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross.Models.Common;
using TFW.Framework.DI;

namespace TFW.WebAPI.Middlewares
{
    [ScopedService]
    public class PrincipalInfoMiddleware : IMiddleware
    {
        private readonly IIdentityService _identityService;

        public PrincipalInfoMiddleware(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var principal = context.User;
            var principalInfo = _identityService.MapToPrincipalInfo(principal);

            PrincipalInfo.Current = principalInfo;

            await next(context);
        }
    }
}
