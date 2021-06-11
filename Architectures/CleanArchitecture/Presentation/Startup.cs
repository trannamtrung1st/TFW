using Application.Abstracts.Data;
using Application.Abstracts.Services;
using Application.Customers.Queries.GetCustomersList;
using Application.Employees.Queries.GetEmployeesList;
using Application.Products.Queries.GetProductsList;
using Application.Sales.Commands.CreateSale.Factories;
using Application.Sales.Queries.GetSaleDetail;
using Application.Sales.Queries.GetSalesList;
using Cross.Dates;
using Infrastructure.Inventory;
using Infrastructure.Network;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation
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
            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString(nameof(DataContext))));

            services.AddMediatR(typeof(Startup),
                typeof(Application.AssemblyModel));

            services.AddScoped<IDbInitializer, DbInitializer>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IInventoryService, InventoryService>()
                .AddScoped<ISaleFactory, SaleFactory>()
                .AddScoped<IDateService, DateService>()
                .AddScoped<IGetCustomersListQuery, GetCustomersListQuery>()
                .AddScoped<IGetEmployeesListQuery, GetEmployeesListQuery>()
                .AddScoped<IGetProductsListQuery, GetProductsListQuery>()
                .AddScoped<IGetSalesListQuery, GetSalesListQuery>()
                .AddScoped<IGetSaleDetailQuery, GetSaleDetailQuery>()
                .AddScoped<IWebClientWrapper, WebClientWrapper>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
