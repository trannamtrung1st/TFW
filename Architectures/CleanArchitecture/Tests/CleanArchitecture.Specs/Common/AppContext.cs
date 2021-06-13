using Application.Abstracts.Data;
using Application.Abstracts.Services;
using Application.Customers.Queries.GetCustomersList;
using Application.Employees.Queries.GetEmployeesList;
using Application.Products.Queries.GetProductsList;
using Application.Sales.Commands.CreateSale.Factories;
using Application.Sales.Queries.GetSaleDetail;
using Application.Sales.Queries.GetSalesList;
using Cross.Dates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence;
using Persistence.Services;
using SolidToken.SpecFlow.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Specs.Common
{
    public static class AppContext
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DataContext>(opt =>
                opt.UseSqlite("Data Source=./CleanArchitecture.Specs.db"));

            services.AddMediatR(typeof(Application.AssemblyModel));

            services.AddScoped<IDbMigrator, DbMigrator>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<ISaleFactory, SaleFactory>()
                .AddScoped<IGetCustomersListQuery, GetCustomersListQuery>()
                .AddScoped<IGetEmployeesListQuery, GetEmployeesListQuery>()
                .AddScoped<IGetProductsListQuery, GetProductsListQuery>()
                .AddScoped<IGetSalesListQuery, GetSalesListQuery>()
                .AddScoped<IGetSaleDetailQuery, GetSaleDetailQuery>()
                .AddScoped(o =>
                {
                    var mock = new Mock<IInventoryService>();
                    return mock.Object;
                })
                .AddScoped(o =>
                {
                    var mock = new Mock<IDateService>();
                    mock.Setup(o => o.GetDate()).Returns(DateTime.Now.Date);
                    return mock.Object;
                });

            return services;
        }
    }
}
