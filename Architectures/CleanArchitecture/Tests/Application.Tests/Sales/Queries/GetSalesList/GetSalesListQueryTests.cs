﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Sales;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Application.Abstracts.Data;

namespace Application.Sales.Queries.GetSalesList.Tests
{
    [TestFixture()]
    public class GetSalesListQueryTests
    {
        [Test()]
        public async Task ExecuteAsyncTest()
        {
            var customer = new Customer
            {
                Id = 1,
                Name = "Abc"
            };

            var employee = new Employee
            {
                Id = 2,
                Name = "Xyz"
            };

            var product = new Product
            {
                Id = 3,
                Name = "p1",
                Price = 10421.421
            };

            var sales = new List<Sale>();
            for (var i = 0; i < 5; i++)
            {
                sales.Add(new Sale(DateTime.Now.AddDays(i), customer, employee, product, i * 10)
                {
                    Id = i + 1
                });
            }

            var saleModels = sales.Select(o => new SalesListItemModel
            {
                UnitPrice = o.UnitPrice,
                CustomerName = o.Customer.Name,
                Date = o.Date,
                EmployeeName = o.Employee.Name,
                Id = o.Id,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                TotalPrice = o.TotalPrice
            }).ToArray();

            var expectedObj = new
            {
                customer,
                employee,
                product,
                sales,
                saleModels
            };

            var saleRepoMock = new Mock<IRepository<Sale>>();

            saleRepoMock.Setup(o => o.Get()).Returns(sales.AsQueryable());

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<SalesListItemModel>>())).Returns((IQueryable<SalesListItemModel> inp) =>
            {
                return Task.FromResult(inp.ToArray());
            });

            var query = new GetSalesListQuery(uowMock.Object, saleRepoMock.Object);

            var results = await query.ExecuteAsync();

            for (var i = 0; i < expectedObj.saleModels.Length; i++)
            {
                var expected = expectedObj.saleModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.CustomerName, actual.CustomerName);
                Assert.AreEqual(expected.Date, actual.Date);
                Assert.AreEqual(expected.EmployeeName, actual.EmployeeName);
                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.ProductName, actual.ProductName);
                Assert.AreEqual(expected.Quantity, actual.Quantity);
                Assert.AreEqual(expected.TotalPrice, actual.TotalPrice);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}