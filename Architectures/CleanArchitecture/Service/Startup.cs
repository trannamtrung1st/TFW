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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Service
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

            services.AddControllers();

            //services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                #region Bearer/Api Key
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[0]
                    }
                });
                #endregion

                var baseDir = AppContext.BaseDirectory;
                var webApiXml = Path.Combine(baseDir, $"{typeof(Startup).Assembly.GetName().Name}.xml");
                var applicationXml = Path.Combine(baseDir, $"{typeof(Application.AssemblyModel).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(webApiXml);
                c.IncludeXmlComments(applicationXml);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

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
                c.InjectStylesheet("/custom-swagger-ui.css");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                //builder.AllowAnyOrigin();
                builder.SetIsOriginAllowed(origin =>
                {
                    return true;
                });
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
