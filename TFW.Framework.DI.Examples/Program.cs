using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using TFW.Framework.DI.Attributes;

namespace TFW.Framework.DI.Examples
{
    class TestDispose : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Dispose");
        }
    }

    class Logger
    {

    }

    [TransientService]
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
            TestDispose();
        }

        static void TestDispose()
        {
            var services = new ServiceCollection()
                .AddSingleton(new TestDispose());

            using var container = services.BuildServiceProvider();

            using (var scope = container.CreateScope())
            {
                var test = scope.ServiceProvider.GetRequiredService<TestDispose>();
                var test2 = scope.ServiceProvider.GetRequiredService<TestDispose>();
            }
            Console.WriteLine("Finish test");
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
            IServiceInjector serviceInjector;
            var services = new ServiceCollection()
                .AddServiceInjector(new[] { typeof(Program).Assembly }, out serviceInjector)
                .AddTransient<Logger>()
                .ScanServices(new[] { typeof(Program).Assembly }, serviceInjector);

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
