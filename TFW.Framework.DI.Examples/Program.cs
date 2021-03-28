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
        }
    }

    class Dispose2 : IDisposable
    {
        public void Dispose()
        {
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
            Autofac(100000);
        }

        static void TestKeyed()
        {
            IKeyedServiceManager manager;

            var services = new ServiceCollection()
                .AddKeyedServiceManager(out manager)
                .AddSingleton<TestDispose>()
                .SetKeyed<IDisposable, TestDispose>(manager, 1, factory: provider => new TestDispose())
                .SetKeyed<IDisposable, Dispose2>(manager, 2);

            using var container = services.BuildServiceProvider();

            using (var scope = container.CreateScope())
            {
                var test = scope.ServiceProvider.GetRequiredService<IDisposable>(1);
                var test2 = scope.ServiceProvider.GetRequiredService<IDisposable>(2);
            }
            Console.WriteLine("Finish test");
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
            IKeyedServiceManager manager;

            var services = new ServiceCollection()
                .AddServiceInjector(new[] { typeof(Program).Assembly }, out serviceInjector)
                .AddKeyedServiceManager(out manager)
                .AddTransient<Logger>()
                .ScanServices(new[] { typeof(Program).Assembly }, serviceInjector)
                .SetKeyed<IDisposable, TestDispose>(manager, 1, factory: DIHelper.BuildInjectedFactory<TestDispose>(), lifetime: ServiceLifetime.Transient)
                .SetKeyed<IDisposable, Dispose2>(manager, 2, factory: DIHelper.BuildInjectedFactory<Dispose2>(), lifetime: ServiceLifetime.Transient);

            var container = services.BuildServiceProvider();

            Console.WriteLine(nameof(TFWPropertyInjection));
            var sw = Stopwatch.StartNew();
            using var scope = container.CreateScope();

            var provider = scope.ServiceProvider;
            for (var i = 0; i < loop; i++)
            {
                var test = provider.GetRequiredService<IDisposable>(1);
                var test2 = provider.GetRequiredService<IDisposable>(2);
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
            builder.RegisterType<TestDispose>().Keyed<IDisposable>(1).PropertiesAutowired();
            builder.RegisterType<Dispose2>().Keyed<IDisposable>(2).PropertiesAutowired();
            var container = builder.Build();

            Console.WriteLine(nameof(Autofac));
            var sw = Stopwatch.StartNew();
            using var scope = container.BeginLifetimeScope();

            for (var i = 0; i < loop; i++)
            {
                var test = scope.ResolveKeyed<IDisposable>(1);
                var test2 = scope.ResolveKeyed<IDisposable>(2);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
