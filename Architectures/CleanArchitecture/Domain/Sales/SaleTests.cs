using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Sales
{
    [TestFixture]
    public class SaleTests
    {
        private Sale _sale;
        private Customer _customer;
        private Employee _employee;
        private Product _product;

        private const int Id = 1;
        private static readonly DateTime Date = new DateTime(2001, 2, 3);
        private const decimal UnitPrice = 1.00m;
        private const int Quantity = 1;

        [SetUp]
        public void SetUp()
        {
            _customer = new Customer();

            _employee = new Employee();

            _product = new Product();

            _sale = new Sale();
        }

        [Test]
        public void TestSetAndGetId()
        {
            _sale.Id = Id;

            Assert.AreEqual(_sale.Id, Id);
        }

        [Test]
        public void TestSetAndGetDate()
        {
            _sale.Date = Date;

            Assert.AreEqual(_sale.Date, Date);
        }

        [Test]
        public void TestSetAndGetCustomer()
        {
            _sale.Customer = _customer;

            Assert.AreEqual(_sale.Customer, _customer);
        }

        [Test]
        public void TestSetAndGetEmployee()
        {
            _sale.Employee = _employee;

            Assert.AreEqual(_sale.Employee, _employee);
        }

        [Test]
        public void TestSetAndGetProduct()
        {
            _sale.Product = _product;

            Assert.AreEqual(_sale.Product, _product);
        }

        [Test]
        public void TestSetAndGetUnitPrice()
        {
            _sale.UnitPrice = UnitPrice;

            Assert.AreEqual(_sale.UnitPrice, UnitPrice);
        }

        [Test]
        public void TestSetAndGetQuantity()
        {
            _sale.Quantity = Quantity;

            Assert.AreEqual(_sale.Quantity, Quantity);
        }

        [Test]
        public void TestSetUnitPriceShouldRecomputeTotalPrice()
        {
            _sale.Quantity = Quantity;

            _sale.UnitPrice = 1.23m;

            Assert.AreEqual(_sale.TotalPrice, Quantity * 1.23m);
        }

        [Test]
        public void TestSetQuantityShouldRecomputeTotalPrice()
        {
            _sale.UnitPrice = UnitPrice;

            _sale.Quantity = 2;

            Assert.AreEqual(_sale.TotalPrice, 2 * UnitPrice);
        }
    }
}
