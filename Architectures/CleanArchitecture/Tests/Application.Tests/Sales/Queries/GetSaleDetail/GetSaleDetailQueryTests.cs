using NUnit.Framework;
using Application.Sales.Queries.GetSaleDetail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Sales;
using Domain.Customers;
using Domain.Employees;
using Domain.Products;
using System.Linq;
using Moq;
using Application.Abstracts.Data;

namespace Application.Sales.Queries.GetSaleDetail.Tests
{
    [TestFixture()]
    public class GetSaleDetailQueryTests
    {
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public async Task ExecuteAsyncTest(int saleId)
        {
            var sales = new List<Sale>();

            var customer = new Customer { Id = 1, Name = "Keven" };
            var employee = new Employee { Id = 1, Name = "Chris" };
            var product = new Product { Id = 1, Name = "P1", Price = 1248123.213 };
            var now = DateTime.Now;

            for (var i = 0; i < 5; i++)
            {
                sales.Add(new Sale(now.AddDays(i), customer, employee, product, (i + 1) * i)
                {
                    Id = i + 1,
                });
            }

            var saleModel = sales.Select(o => new SaleDetailModel
            {
                CustomerName = o.Customer.Name,
                Date = o.Date,
                EmployeeName = o.Employee.Name,
                Id = o.Id,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                TotalPrice = o.TotalPrice,
                UnitPrice = o.UnitPrice
            }).Single(o => o.Id == saleId);

            var expectedObj = new
            {
                saleId,
                sales,
                saleModel,
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.SingleOrDefaultAsync(It.IsAny<IQueryable<SaleDetailModel>>()))
                .Returns((IQueryable<SaleDetailModel> inp) => Task.FromResult(inp.Single()));

            var saleRepoMock = new Mock<IRepository<Sale>>();

            saleRepoMock.Setup(o => o.Get()).Returns(sales.AsQueryable());

            var query = new GetSaleDetailQuery(uowMock.Object, saleRepoMock.Object);

            var actual = await query.ExecuteAsync(expectedObj.saleId);

            var expected = expectedObj.saleModel;

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