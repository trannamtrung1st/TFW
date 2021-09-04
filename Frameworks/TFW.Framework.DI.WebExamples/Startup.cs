using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TFW.Framework.DI.WebExamples.Builders;
using TFW.Framework.DI.WebExamples.Models;
using TFW.Framework.DI.WebExamples.Repositories;

namespace TFW.Framework.DI.WebExamples
{
    public interface IAppSettings
    {
        public string AppName { get; }
    }

    public class AppSettings : IAppSettings
    {
        public string AppName { get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("DependencyInjection"));

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddTransient<IProductBuilder, MyProductBuilder>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddSingleton<IAppSettings>(appSettings);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            #region Init data
            dataContext.Product.AddRange(new Product
            {
                Id = 1,
                Name = "IPhone"
            }, new Product
            {
                Id = 2,
                Name = "IPad"
            }, new Product
            {
                Id = 3,
                Name = "Samsung Galaxy"
            });

            dataContext.SaveChanges();
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
