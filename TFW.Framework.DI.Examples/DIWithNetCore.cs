using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.DI.Examples
{
    class DIWithNetCore
    {
        interface IEarth
        {
        }

        class Earth : IEarth
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

            // a list of ServiceDescriptors
            services.AddSingleton<IEarth, Earth>()
                .AddTransient<Children>()
                .AddScoped<Parent>()
                .Replace(new ServiceDescriptor(typeof(IEarth), new Earth()))
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

            // resolve from root
            IEarth earth = serviceProvider.GetService<IEarth>();
            IEnumerable<IEarth> earths = serviceProvider.GetServices<IEarth>();

            using var scope = serviceProvider.CreateScope();

            // resolve from scope
            Children children = scope.ServiceProvider.GetRequiredService<Children>();
            Parent parent = scope.ServiceProvider.GetRequiredService<Parent>();
        }
    }
}
