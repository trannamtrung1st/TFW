using DeviceId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices;

namespace TFW.IdentifierService
{
    public class Program
    {
        private static string _deviceId;
        public static void Main(string[] args)
        {
            _deviceId = new DeviceIdBuilder()
               .AddMachineName()
               .AddProcessorId()
               .AddMotherboardSerialNumber()
               .AddSystemDriveSerialNumber()
               .ToString();

            var builder = CreateHostBuilder(args);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder.UseWindowsService();
            }

            builder.Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHost(builder =>
                    builder.UseKestrel()
                        .ConfigureServices(services => services.AddCors())
                        .Configure(app =>
                        {
                            app.UseCors(builder =>
                            {
                                builder.WithMethods(HttpMethods.Get);
                                builder.AllowCredentials();
                                builder.SetIsOriginAllowed(origin =>
                                {
                                    return true;
                                });
                            });

                            app.Run(r => r.Response.WriteAsync(_deviceId));
                        }));
    }
}
