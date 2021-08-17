using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Background.Auth
{
    public class AppAuthorizeFilter : IDashboardAsyncAuthorizationFilter
    {
        public Task<bool> AuthorizeAsync([NotNull] DashboardContext context)
        {
            return Task.FromResult(
                context.GetHttpContext().User.Identity.IsAuthenticated);
        }
    }
}
