using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TFW.Docs.ApiClient;
using TFW.Docs.AppAdmin.Models;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.Configuration.Extensions;

namespace TFW.Docs.AppAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Settings
            Settings.Set(Configuration.Parse<WebAppSettings>(nameof(WebAppSettings)));
            Settings.Set(Configuration.Parse<AppSettings>(nameof(AppSettings)));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var webAppSettings = Settings.Get<WebAppSettings>();

            services.ConfigureAppOptions(Configuration)
                .AddApiClient(webAppSettings.ApiBase, AppClients.TFWDocsWebApp)
                .AddMemoryCache()
                .AddWebFrameworks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiClient apiClient,
            IMemoryCache memoryCache)
        {
            ApiEndpoints.BaseUrl = Settings.Get<WebAppSettings>().ApiBase;

            app.UseExceptionHandler(Routing.App.Status);
            app.UseStatusCodePagesWithReExecute(Routing.App.Status, $"?{Parameters.StatusParam}={{0}}");

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            InitializeAsync(apiClient, memoryCache).Wait();
        }

        private async Task InitializeAsync(IApiClient apiClient, IMemoryCache memoryCache)
        {
            var initStatus = await apiClient.Setting.GetInitStatusAsync();
            memoryCache.Set(CachingKeys.InitStatus, initStatus.Result.Data);
        }
    }
}
