using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Sales.Commands.CreateSale.Factories
{
    [TestFixture]
    public class SaleFactoryTests
    {
        private ISaleFactory _factory;
        private Customer _customer;
        private Employee _employee;
        private Product _product;

        private static readonly DateTime DateTime = new DateTime(2001, 2, 3);
        private const int Quantity = 123;
        private const decimal Price = 1.00m;


        [SetUp]
        public void SetUp()
        {
            _customer = new Customer();

            _employee = new Employee();

            _product = new Product
            {
                Price = Price
            };

            _factory = new SaleFactory();
        }

        [Test]
        public void TestCreateShouldCreateSale()
        {
            var result = _factory.Create(DateTime, _customer, _employee, _product, Quantity);

            Assert.AreEqual(result.Date, DateTime);
            Assert.AreEqual(result.Customer, _customer);
            Assert.AreEqual(result.Employee, _employee);
            Assert.AreEqual(result.Product, _product);
            Assert.AreEqual(result.UnitPrice, Price);
            Assert.AreEqual(result.Quantity, Quantity);
        }
    }
}
