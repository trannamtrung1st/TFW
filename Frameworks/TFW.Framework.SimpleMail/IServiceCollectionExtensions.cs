using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.SimpleMail
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSmtpService(this IServiceCollection services, IConfiguration smtpConfigSection)
        {
            return services.Configure<SmtpOption>(smtpConfigSection)
                .AddScoped<ISmtpService, SmtpService>();
        }

        public static IServiceCollection AddSmtpService(this IServiceCollection services, Action<SmtpOption> config)
        {
            return services.Configure(config)
                .AddScoped<ISmtpService, SmtpService>();
        }
    }
}
