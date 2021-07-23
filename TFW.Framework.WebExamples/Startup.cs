using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.WebExamples.Entities;
using TFW.Framework.WebExamples.Filters;

namespace TFW.Framework.WebExamples
{
    public class Startup
    {
        public static Settings Settings { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings = new Settings();
            configuration.Bind(nameof(Settings), Settings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString(nameof(DataContext))));

            services.AddControllers();
            services.AddRazorPages(opt =>
            {
                // must use cookies and headers AntiForegery instead of FormData to prevent error
                opt.Conventions.AddPageApplicationModelConvention("/Upload/Index",
                    model =>
                    {
                        model.Filters.Add(new GenerateAntiforgeryTokenCookieAttribute());
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
