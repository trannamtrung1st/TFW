using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.Async;

namespace TFW.Framework.Logging.Examples
{
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

    public static class SerilogExamples
    {
        public static void Run()
        {
            var levelSwitch = new LoggingLevelSwitch(initialMinimumLevel: LogEventLevel.Debug);

            SelfLog.Enable(Console.Out);
            SelfLog.WriteLine("This is a self log");

            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {LoggerName} {ClientIp} {Message:lj}{Username:j}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Error)
                .Enrich.WithThreadId()
                .Enrich.WithClientIp()
                .Enrich.FromLogContext()
                .Enrich.With<TestEnricher>()
                .Destructure.ByTransforming<TestEnricher>(o => "He is enricher")
#if false
                .Destructure.With<DestructorPolicy>()
#endif
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
            enricherContext.Information("Connected to {Enricher} {SourceContext} {ContextVal}", test); // wrong use of property

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
