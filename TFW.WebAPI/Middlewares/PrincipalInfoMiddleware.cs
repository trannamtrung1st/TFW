using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Framework.DI;

namespace TFW.WebAPI.Middlewares
{
    [TransientService]
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
            context.Items[ControllerConsts.PrincipalInfoItemKey] = principalInfo;

            await next(context);
        }
    }
}
