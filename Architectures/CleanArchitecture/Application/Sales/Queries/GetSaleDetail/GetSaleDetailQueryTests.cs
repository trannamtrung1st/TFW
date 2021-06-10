using Application.Abstracts.Data;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using Domain.Sales;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sales.Queries.GetSaleDetail
{
    [TestFixture]
    public class GetSaleDetailQueryTests
    {
        private GetSaleDetailQuery _query;
        private Sale _sale;

        private const int SaleId = 1;
        private static readonly DateTime Date = new DateTime(2001, 2, 3);
        private const string CustomerName = "Customer 1";
        private const string EmployeeName = "Employee 1";
        private const string ProductName = "Product 1";
        private const decimal UnitPrice = 1.23m;
        private const int Quantity = 2;
        private const decimal TotalPrice = 2.46m;

        [SetUp]
        public void SetUp()
        {
            var customer = new Customer
            {
                Name = CustomerName
            };

            var employee = new Employee
            {
                Name = EmployeeName
            };

            var product = new Product
            {
                Name = ProductName
            };

            _sale = new Sale()
            {
                Id = SaleId,
                Date = Date,
                Customer = customer,
                Employee = employee,
                Product = product,
                UnitPrice = UnitPrice,
                Quantity = Quantity
            };

            var saleModel = new SaleDetailModel()
            {
                Id = _sale.Id,
                Date = _sale.Date,
                CustomerName = _sale.Customer.Name,
                EmployeeName = _sale.Employee.Name,
                ProductName = _sale.Product.Name,
                UnitPrice = _sale.UnitPrice,
                Quantity = _sale.Quantity,
                TotalPrice = _sale.TotalPrice
            };

            var queryable = new[] { saleModel }.AsQueryable();
            var repoMock = new Mock<IRepository<Sale>>();

            repoMock.Setup(p => p.Get())
                .Returns(() => new[] { _sale }.AsQueryable());

            repoMock.Setup(p => p.SingleOrDefaultAsync(queryable))
                .Returns(() => Task.FromResult(saleModel));

            _query = new GetSaleDetailQuery(repoMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldReturnListOfSales()
        {
            var result = await _query.ExecuteAsync(SaleId);

            Assert.AreEqual(result.Id, SaleId);
            Assert.AreEqual(result.Date, Date);
            Assert.AreEqual(result.CustomerName, CustomerName);
            Assert.AreEqual(result.EmployeeName, EmployeeName);
            Assert.AreEqual(result.ProductName, ProductName);
            Assert.AreEqual(result.UnitPrice, UnitPrice);
            Assert.AreEqual(result.Quantity, Quantity);
            Assert.AreEqual(result.TotalPrice, TotalPrice);
        }
    }
}
