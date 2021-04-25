using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Extensions;
using TFW.Framework.DI.Attributes;

namespace TFW.Docs.WebApi.Middlewares
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

            if (principalInfo.UserId != 0)
                _diagnosticContext.Set(LoggingConsts.Properties.UserId, principalInfo.UserId);

            await next(context);
        }
    }
}
