using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Helpers;
using TFW.Framework.DI.Attributes;

namespace TFW.WebAPI.Middlewares
{
    [ScopedService]
    public class RequestDataExtractionMiddleware : IMiddleware
    {
        private readonly IIdentityService _identityService;
        private readonly IDiagnosticContext _diagnosticContext;

        public RequestDataExtractionMiddleware(IIdentityService identityService,
            IDiagnosticContext diagnosticContext)
        {
            _identityService = identityService;
            _diagnosticContext = diagnosticContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var principal = context.User;
            var principalInfo = _identityService.MapToPrincipalInfo(principal);

            context.SetPrincipalInfo(principalInfo);

            if (principalInfo.UserId != null)
                _diagnosticContext.Set(ConfigConsts.Logging.Properties.UserId, principalInfo.UserId);

            await next(context);
        }
    }
}
