﻿using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Tests.Common.Data
{
    public static class DataSets
    {
        private static IDictionary<string, Func<CleanArchitectureDataSet>> _dataSets = new Dictionary<string, Func<CleanArchitectureDataSet>>();

        public static bool Contains(string key)
        {
            return _dataSets.ContainsKey(key);
        }

        public static CleanArchitectureDataSet Get(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            return _dataSets[key]();
        }

        public static void Set(string key, CleanArchitectureDataSet model)
        {
            _dataSets[key] = () =>
            {
                var dataSet = new CleanArchitectureDataSet();

                dataSet.Customers = model.Customers.Select(o => new Customer
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

                dataSet.Employees = model.Employees.Select(o => new Employee
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

                dataSet.Products = model.Products.Select(o => new Product
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price
                }).ToArray();

                dataSet.Sales = model.Sales.Select(sale => new Sale
                {
                    Id = sale.Id,
                    Customer = dataSet.Customers.Single(o => o.Name == sale.Customer.Name),
                    Date = sale.Date,
                    Employee = dataSet.Employees.Single(o => o.Name == sale.Employee.Name),
                    Product = dataSet.Products.Single(o => o.Name == sale.Product.Name),
                    Quantity = sale.Quantity,
                    UnitPrice = sale.UnitPrice
                }).ToArray();

                return dataSet;
            };
        }
    }
}
