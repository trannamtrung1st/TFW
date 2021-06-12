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

            var expectedObj = new
            {
                now,
                customer,
                employee,
                product,
                quantity,
                totalPrice = product.Price * quantity
            };

            var sale = new Sale(expectedObj.now,
                expectedObj.customer,
                expectedObj.employee,
                expectedObj.product,
                expectedObj.quantity);

            Assert.AreEqual(expectedObj.product.Price, sale.UnitPrice);
            Assert.AreEqual(expectedObj.totalPrice, sale.TotalPrice);
        }

        [TestCase(12.5, 2)]
        [TestCase(500, 25)]
        [TestCase(51200, 10)]
        public void SetUnitPriceShouldUpdateTotalPrice(double unitPrice, int quantity)
        {
            var expectedObj = new
            {
                unitPrice,
                quantity,
                prevPrice = 0,
                prevTotal = 0,
                afterPrice = unitPrice,
                afterTotal = unitPrice * quantity
            };

            var sale = new Sale();
            sale.Quantity = expectedObj.quantity;

            Assert.AreEqual(expectedObj.prevPrice, sale.UnitPrice);
            Assert.AreEqual(expectedObj.prevTotal, sale.TotalPrice);

            sale.UnitPrice = expectedObj.unitPrice;

            Assert.AreEqual(expectedObj.afterPrice, sale.UnitPrice);
            Assert.AreEqual(expectedObj.afterTotal, sale.TotalPrice);
        }

        [TestCase(12.5, 2)]
        [TestCase(500, 25)]
        [TestCase(51200, 10)]
        public void SetQuantityShouldUpdateTotalPrice(double unitPrice, int quantity)
        {
            var expectedObj = new
            {
                unitPrice,
                quantity,
                prevQuantity = 0,
                prevTotal = 0,
                afterQuantity = quantity,
                afterTotal = unitPrice * quantity
            };

            var sale = new Sale();
            sale.UnitPrice = expectedObj.unitPrice;

            Assert.AreEqual(expectedObj.prevQuantity, sale.Quantity);
            Assert.AreEqual(expectedObj.prevTotal, sale.TotalPrice);

            sale.Quantity = expectedObj.quantity;

            Assert.AreEqual(expectedObj.afterQuantity, sale.Quantity);
            Assert.AreEqual(expectedObj.afterTotal, sale.TotalPrice);
        }
    }
}