using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace TFW.Framework.Logging.Examples
{
    public class TestEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Username", Guid.NewGuid().ToString()));
        }
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
            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {LoggerName} {Message:lj}{Username:j}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With<TestEnricher>()
                .Destructure.ByTransforming<TestEnricher>(o => "He is enricher")
#if false
                .Destructure.With<DestructorPolicy>()
#endif
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour, outputTemplate: template)
#if false
                .WriteTo.Logger(cfg =>
                    cfg.Filter.ByExcluding(Matching.WithProperty<string>("Username", username => username.Contains("z")))
                        .Enrich.WithProperty("LoggerName", "VCL")
                        .WriteTo.Console(outputTemplate: template))
#endif
                .CreateLogger();

            var test = new TestEnricher();
            Log.Information("Connected to {Enricher}", test);

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
