using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Examples
{
    class DIWithNetCore
    {
        class Earth
        {

        }

        class Children
        {

        }

        class Parent
        {
            private Children _children;

            public Parent(Children children)
            {
                _children = children;
            }
        }

        static void Registration()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<Earth>()
                .AddTransient<Children>()
                .AddScoped<Parent>()
                .TryAddScoped(provider =>
                {
                    var child = provider.GetRequiredService<Children>();
                    var parent = new Parent(child);
                    return parent;
                });

            IServiceProvider serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });
        }
    }
}
