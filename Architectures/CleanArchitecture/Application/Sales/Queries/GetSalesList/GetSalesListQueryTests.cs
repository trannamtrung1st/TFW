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

namespace Application.Sales.Queries.GetSalesList
{
    [TestFixture]
    public class GetSalesListQueryTests
    {
        private GetSalesListQuery _query;
        private Sale _sale;

        private const int SaleId = 1;
        private static readonly DateTime Date = new DateTime(2001, 2, 3);
        private const string CustomerName = "Customer 1";
        private const string EmployeeName = "Employee 1";
        private const string ProductName = "Product 1";
        private const double UnitPrice = 1.23;
        private const int Quantity = 2;
        private const double TotalPrice = 2.46;

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

            var saleArr = new[] { _sale };
            var saleModelArr = new[]
            {
                new SalesListItemModel()
                {
                    Id = _sale.Id,
                    Date = _sale.Date,
                    CustomerName = _sale.Customer.Name,
                    EmployeeName = _sale.Employee.Name,
                    ProductName = _sale.Product.Name,
                    UnitPrice = _sale.UnitPrice,
                    Quantity = _sale.Quantity,
                    TotalPrice = _sale.TotalPrice
                }
            };

            var queryable = saleModelArr.AsQueryable();
            var repoMock = new Mock<IRepository<Sale>>();
            var uowMock = new Mock<IUnitOfWork>();

            repoMock.Setup(p => p.Get())
                .Returns(() => saleArr.AsQueryable());

            uowMock.Setup(p => p.ToArrayAsync(queryable))
                .Returns(() => Task.FromResult(saleModelArr));

            _query = new GetSalesListQuery(uowMock.Object, repoMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldReturnListOfSales()
        {
            var results = await _query.ExecuteAsync();

            var result = results.Single();

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
