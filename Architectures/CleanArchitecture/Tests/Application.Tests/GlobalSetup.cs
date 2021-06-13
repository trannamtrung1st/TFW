using Application.Tests.Common.Data;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var dSet = new CleanArchitectureDataSet
        {
            Customers = new[]
            {
                new Customer() { Id = 1, Name = "Martin Fowler" },
                new Customer() { Id = 2, Name = "Uncle Bob" },
                new Customer() { Id = 3, Name = "Kent Beck" }
            },
            Employees = new[]
            {
                new Employee() { Id = 1, Name = "Eric Evans" },
                new Employee() { Id = 2, Name = "Greg Young" },
                new Employee() { Id = 3, Name = "Udi Dahan" }
            },
            Products = new[]
            {
                new Product() { Id = 1, Name = "Spaghetti", Price = 5 },
                new Product() { Id = 2, Name = "Lasagna", Price = 10 },
                new Product() { Id = 3, Name = "Ravioli", Price = 15 },
            }
        };

        var sales = new List<Sale>();

        sales.Add(new Sale()
        {
            Id = 1,
            Date = DateTime.Now.Date,
            Customer = dSet.Customers.ElementAt(0),
            Employee = dSet.Employees.ElementAt(0),
            Product = dSet.Products.ElementAt(0),
            UnitPrice = 5,
            Quantity = 1
        });

        sales.Add(new Sale()
        {
            Id = 2,
            Date = DateTime.Now.Date,
            Customer = dSet.Customers.ElementAt(1),
            Employee = dSet.Employees.ElementAt(1),
            Product = dSet.Products.ElementAt(1),
            UnitPrice = 10,
            Quantity = 2
        });

        sales.Add(new Sale()
        {
            Id = 3,
            Date = DateTime.Now.Date,
            Customer = dSet.Customers.ElementAt(2),
            Employee = dSet.Employees.ElementAt(2),
            Product = dSet.Products.ElementAt(2),
            UnitPrice = 15,
            Quantity = 3
        });

        dSet.Sales = sales;

        DataSets.Set("default", dSet);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}
