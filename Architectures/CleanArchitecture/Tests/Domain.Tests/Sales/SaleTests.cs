using NUnit.Framework;
using Domain.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Customers;
using Domain.Products;
using Domain.Employees;
using System.Linq;

namespace Domain.Sales.Tests
{
    [TestFixture()]
    public class SaleTests
    {
        private Customer[] _customers;
        private Employee[] _employees;
        private Product[] _products;

        [SetUp]
        public void Setup()
        {
            _customers = new[]
            {
                new Customer{ Id = 1, Name = Guid.NewGuid().ToString() },
                new Customer{ Id = 2, Name = Guid.NewGuid().ToString() },
            };

            _employees = new[]
            {
                new Employee{ Id = 1, Name = Guid.NewGuid().ToString() },
                new Employee{ Id = 2, Name = Guid.NewGuid().ToString() },
            };

            _products = new[]
            {
                new Product{ Id = 1, Name = Guid.NewGuid().ToString(), Price = 12 },
                new Product{ Id = 2, Name = Guid.NewGuid().ToString(), Price = 13 },
                new Product{ Id = 3, Name = Guid.NewGuid().ToString(), Price = 25.3 },
            };
        }

        [Test()]
        public void SaleTest()
        {
            _ = new Sale();
        }

        [TestCase(1, 1, 2, 1)]
        [TestCase(2, 2, 1, 50)]
        [TestCase(2, 1, 3, 100)]
        public void GetTotalPriceTest(int customerId, int employeeId, int productId, int quantity)
        {
            var now = DateTime.Now;
            var customer = _customers.Single(c => c.Id == customerId);
            var employee = _employees.Single(e => e.Id == employeeId);
            var product = _products.Single(p => p.Id == productId);

            var sale = new Sale(now, customer, employee, product, quantity);

            Assert.AreEqual(product.Price, sale.UnitPrice);
            Assert.AreEqual(product.Price * quantity, sale.TotalPrice);
        }

        [TestCase(12.5, 2)]
        [TestCase(500, 25)]
        [TestCase(51200, 10)]
        public void SetUnitPriceShouldUpdateTotalPrice(double unitPrice, int quantity)
        {
            var sale = new Sale();
            sale.Quantity = quantity;

            Assert.AreEqual(0, sale.UnitPrice);
            Assert.AreEqual(0, sale.TotalPrice);

            sale.UnitPrice = unitPrice;

            Assert.AreEqual(unitPrice, sale.UnitPrice);
            Assert.AreEqual(unitPrice * quantity, sale.TotalPrice);
        }

        [TestCase(12.5, 2)]
        [TestCase(500, 25)]
        [TestCase(51200, 10)]
        public void SetQuantityShouldUpdateTotalPrice(double unitPrice, int quantity)
        {
            var sale = new Sale();
            sale.UnitPrice = unitPrice;

            Assert.AreEqual(0, sale.Quantity);
            Assert.AreEqual(0, sale.TotalPrice);

            sale.Quantity = quantity;

            Assert.AreEqual(quantity, sale.Quantity);
            Assert.AreEqual(unitPrice * quantity, sale.TotalPrice);
        }
    }
}