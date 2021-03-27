using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using TFW.Framework.DI.Attributes;

namespace TFW.Framework.DI.Examples
{
    class Logger
    {

    }

    class Service
    {
        [Inject]
        public Logger Logger { get; set; }
        private int _id;
    }

    class NormalService
    {
        public Logger Logger { get; set; }
        private int _id;

        public NormalService(Logger logger)
        {
            Logger = logger;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Autofac(100000);
        }

        static void ServiceProvider(int loop)
        {
            var services = new ServiceCollection()
                .AddServiceInjector(new[] { typeof(Program).Assembly })
                .AddTransient<Logger>()
                .AddTransient<NormalService>();

            var container = services.BuildServiceProvider();

            Console.WriteLine(nameof(ServiceProvider));
            var sw = Stopwatch.StartNew();
            using var scope = container.CreateScope();

            var provider = scope.ServiceProvider;
            for (var i = 0; i < loop; i++)
            {
                var test = provider.GetRequiredService<NormalService>();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        static void TFWPropertyInjection(int loop)
        {
            var services = new ServiceCollection()
                .AddServiceInjector(new[] { typeof(Program).Assembly })
                .AddTransient<Logger>()
                .AddTransient(DIHelper.BuildInjectedFactory<Service>());

            var container = services.BuildServiceProvider();

            Console.WriteLine(nameof(TFWPropertyInjection));
            var sw = Stopwatch.StartNew();
            using var scope = container.CreateScope();

            var provider = scope.ServiceProvider;
            for (var i = 0; i < loop; i++)
            {
                var test = provider.GetRequiredService<Service>();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        static void Autofac(int loop)
        {
            var builder = new ContainerBuilder();
            // Register individual components
            var injector = new ServiceInjector();
            injector.Register(new[] { typeof(Program).Assembly });
            builder.RegisterInstance<IServiceInjector>(injector);
            builder.RegisterType<Logger>();
            builder.RegisterType<Service>().PropertiesAutowired();
            var container = builder.Build();

            Console.WriteLine(nameof(Autofac));
            var sw = Stopwatch.StartNew();
            using var scope = container.BeginLifetimeScope();

            for (var i = 0; i < loop; i++)
            {
                var test = scope.Resolve<Service>();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
