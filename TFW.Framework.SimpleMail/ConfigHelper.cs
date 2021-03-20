﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.SimpleMail.Options;
using TFW.Framework.SimpleMail.Services;

namespace TFW.Framework.SimpleMail
{
    public static class ConfigHelper
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
