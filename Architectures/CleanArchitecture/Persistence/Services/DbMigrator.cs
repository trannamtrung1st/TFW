﻿using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Services
{
    public class DbMigrator : IDbMigrator
    {
        protected readonly DataContext _dbContext;

        public DbMigrator(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DropAsync()
        {
            return _dbContext.Database.EnsureDeletedAsync();
        }

        public async Task InitAsync()
        {
            var isFirstTime = !(await _dbContext.Database.GetAppliedMigrationsAsync()).Any();

            await _dbContext.Database.MigrateAsync();

            if (isFirstTime)
            {
                await CreateCustomersAsync();

                await CreateEmployeesAsync();

                await CreateProductsAsync();

                await CreateSalesAsync();
            }
        }

        private Task CreateCustomersAsync()
        {
            _dbContext.Customers.Add(new Customer() { Id = 1, Name = "Martin Fowler" });

            _dbContext.Customers.Add(new Customer() { Id = 2, Name = "Uncle Bob" });

            _dbContext.Customers.Add(new Customer() { Id = 3, Name = "Kent Beck" });

            return _dbContext.SaveChangesAsync();
        }

        private Task CreateEmployeesAsync()
        {
            _dbContext.Employees.Add(new Employee() { Id = 1, Name = "Eric Evans" });

            _dbContext.Employees.Add(new Employee() { Id = 2, Name = "Greg Young" });

            _dbContext.Employees.Add(new Employee() { Id = 3, Name = "Udi Dahan" });

            return _dbContext.SaveChangesAsync();
        }

        private Task CreateProductsAsync()
        {
            _dbContext.Products.Add(new Product() { Id = 1, Name = "Spaghetti", Price = 5 });

            _dbContext.Products.Add(new Product() { Id = 2, Name = "Lasagna", Price = 10 });

            _dbContext.Products.Add(new Product() { Id = 3, Name = "Ravioli", Price = 15 });

            return _dbContext.SaveChangesAsync();
        }

        private async Task CreateSalesAsync()
        {
            var customers = await _dbContext.Customers.ToArrayAsync();

            var employees = await _dbContext.Employees.ToArrayAsync();

            var products = await _dbContext.Products.ToArrayAsync();

            _dbContext.Sales.Add(new Sale()
            {
                Id = 1,
                Date = DateTime.Now.Date,
                Customer = customers[0],
                Employee = employees[0],
                Product = products[0],
                UnitPrice = 5,
                Quantity = 1
            });

            _dbContext.Sales.Add(new Sale()
            {
                Id = 2,
                Date = DateTime.Now.Date,
                Customer = customers[1],
                Employee = employees[1],
                Product = products[1],
                UnitPrice = 10,
                Quantity = 2
            });

            _dbContext.Sales.Add(new Sale()
            {
                Id = 3,
                Date = DateTime.Now.Date,
                Customer = customers[2],
                Employee = employees[2],
                Product = products[2],
                UnitPrice = 15,
                Quantity = 3
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
