using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.DI.Examples
{
    class Logger
    {

    }

    class Service
    {
        private Logger _logger;
        private int _id;

        public Service(Logger logger)
        {
            _logger = logger;
        }

        [ActivatorUtilitiesConstructor]
        public Service(Logger logger, int id) : this(logger)
        {
            _id = id;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var objectFactory = ActivatorUtilities.CreateFactory(typeof(Service), new Type[] { });

            IServiceCollection services = new ServiceCollection()
                .AddScoped<Logger>()
                .AddScoped<Service>(provider =>
                {
                    var service = objectFactory(provider, new object[] { });

                    // create a proxy if I want

                    return service as Service;
                });

            var provider = services.BuildServiceProvider();

            var service = provider.GetRequiredService<Service>();
        }
    }
}
