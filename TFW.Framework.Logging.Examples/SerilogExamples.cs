using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using Serilog.Configuration;
using Serilog.Context;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Async;
using TFW.Framework.Configuration.Helpers;

namespace TFW.Framework.Logging.Examples
{
    public class CustomSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
            Console.WriteLine(logEvent);
        }
    }

    public class TestEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Username", Guid.NewGuid().ToString()));
        }
    }

    class MonitorConfiguration : IAsyncLogEventSinkMonitor
    {
        public void StartMonitoring(IAsyncLogEventSinkInspector inspector) =>
            inspector.ToString();

        public void StopMonitoring(IAsyncLogEventSinkInspector inspector)
        { /* reverse of StartMonitoring */ }
    }

    public class DestructorPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            result = propertyValueFactory.CreatePropertyValue("Override all destructuring");

            return true;
        }
    }

    public static class Test
    {
        public static void Log(object message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string fileName = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            // we'll just use a simple Console write for now    
            Console.WriteLine("{0}({1}):{2} - {3}", fileName, lineNumber, memberName, message);
        }
    }

    public static class SerilogExamples
    {
        public static LoggerConfiguration CustomSink(
        this LoggerSinkConfiguration writeto, LogEventLevel restricted = LogEventLevel.Verbose,
            LoggingLevelSwitch logSwitch = null)
        {
            return writeto.Sink<CustomSink>(restrictedToMinimumLevel: restricted, levelSwitch: logSwitch);
        }

        public static LoggerConfiguration WithTestEnricher(
        this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<TestEnricher>();
        }

        public static void Run()
        {
            Test.Log("Caller oke");

            SelfLog.Enable(Console.Out);
            SelfLog.WriteLine("This is a self log");

#if true
            var depContext = DependencyContext.Load(typeof(SerilogExamples).Assembly);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serilogCfgSection = configuration.GetSection("Serilog");

            var config = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, sectionName: "Serilog", dependencyContext: depContext);

            string enricherTransform;

            if (serilogCfgSection.TryParse<string>(out enricherTransform, key: "TransformTestEnricher", trans => trans != null))
                config = config.Destructure.ByTransforming<TestEnricher>(o => enricherTransform);

            Log.Logger = config.CreateLogger();
#endif

#if false
            var levelSwitch = new LoggingLevelSwitch(initialMinimumLevel: LogEventLevel.Debug);
            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {LoggerName} {ClientIp} {Message:lj}{Username:j}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Error)
                .Enrich.WithThreadId()
                .Enrich.WithClientIp()
                .Enrich.FromLogContext()
                .Enrich.WithTestEnricher()
                .Destructure.ByTransforming<TestEnricher>(o => "He is enricher")
#if false
                .Destructure.With<DestructorPolicy>()
#endif
                .WriteTo.CustomSink()
                .WriteTo.Console(levelSwitch: levelSwitch, outputTemplate: template, formatProvider: CultureInfo.CurrentCulture)
                .WriteTo.Async(cfg => cfg.File("logs/async.txt"), bufferSize: 1000, blockWhenFull: true, monitor: new MonitorConfiguration())
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour, outputTemplate: template)
#if false
                .WriteTo.Logger(cfg =>
                    cfg.Filter.ByExcluding(Matching.WithProperty<string>("Username", username => username.Contains("z")))
                        .Enrich.WithProperty("LoggerName", "VCL")
                        .WriteTo.Console(outputTemplate: template))
#endif
                .CreateLogger();
#endif

            using (LogContext.PushProperty("A", 1))
            {
                Log.Information("Carries property A = 1");

                using (LogContext.PushProperty("A", 2))
                using (LogContext.PushProperty("B", 1))
                using (LogContext.PushProperty("ClientIp", "11.1..1.1.1.11"))
                {
                    Log.Information("Carries A = 2 and B = 1 {A}"); // wrong use of property
                }

                Log.Information("Carries property A = 1, again");
            }

            var test = new TestEnricher();
            var enricherContext = Log.ForContext<TestEnricher>()
                .ForContext("ContextVal", "OKEOKE");
            enricherContext.Information("Connected to {@Enricher} {SourceContext} {ContextVal}", test); // wrong use of property

            var count = 456;
            Log.Information("Retrieved {Count} records", count);

            var sensorInput = new { Latitude = 25, Longitude = 134 };
            Log.Information("Processing {@SensorInputDes} {SensorInputRaw}", sensorInput, sensorInput);

            var fruit = new[] { "Apple", "Pear", "Orange" };
            Log.Information("In my bowl I have {Fruit}", fruit);

            var fruits = new Dictionary<string, int> { { "Apple", 1 }, { "Pear", 5 } };
            Log.Information("In my bowl I have {Fruit}", fruits);

            Log.Information("Hello, world!");

            var unknown = new[] { 1, 2, 3 };
            Log.Information("Received {$Data}", unknown);

            int a = 10, b = 0;
            try
            {
                Log.Debug("Dividing {A} by {B}", a, b);
                Console.WriteLine(a / b);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
