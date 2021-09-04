using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TFW.Framework.CQRSExamples.Entities.Query;
using TFW.Framework.CQRSExamples.Entities.Relational;
using TFW.Framework.CQRSExamples.Models.Command;
using TFW.Framework.CQRSExamples.Models.Query;
using TFW.Framework.CQRSExamples.Queries;

namespace TFW.Framework.CQRSExamples
{
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
            services.AddDbContext<RelationalContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString(nameof(RelationalContext))));

            services.AddDbContext<QueryDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString(nameof(QueryDbContext))));

            services.AddScoped<IProductCategoryQuery, ProductCategoryQuery>()
                .AddScoped<IProductQuery, ProductQuery>()
                .AddScoped<IOrderQuery, OrderQuery>()
                .AddScoped<IReportQuery, ReportQuery>()
                .AddScoped<IStoreQuery, StoreQuery>()
                .AddScoped<IBrandQuery, BrandQuery>();

            services.AddMediatR(typeof(Startup));

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();

            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(30);
                opt.Cookie.IsEssential = true;
                opt.Cookie.HttpOnly = true;
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            RelationalContext relationalContext,
            QueryDbContext queryDbContext,
            IMediator mediator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            relationalContext.Database.Migrate();
            queryDbContext.Database.Migrate();
            mediator.Send(new InitCommand()).Wait();
        }
    }
}
