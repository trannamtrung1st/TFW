using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Sales;
using System.Linq;
using Moq;
using Application.Abstracts.Data;
using Application.Tests.Common.Data;
using Cross.Tests;

namespace Application.Sales.Queries.GetSaleDetail.Tests
{
    [TestFixture()]
    public class GetSaleDetailQueryTests
    {
        [TestCase(DataSetKeys.Default, 1)]
        [TestCase(DataSetKeys.Default, 2)]
        [TestCase(DataSetKeys.Default, 3)]
        public async Task ExecuteAsyncTest(string dataSetKey, int saleId)
        {
            var dSet = DataSets.Get(dataSetKey);

            var saleModel = dSet.Sales.Select(o => new SaleDetailModel
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
                sales = dSet.Sales,
                saleModel,
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.SingleOrDefaultAsync(It.IsAny<IQueryable<SaleDetailModel>>()))
                .ReturnsAsync((IQueryable<SaleDetailModel> inp) => inp.Single());

            var saleRepoMock = new Mock<IRepository<Sale>>();

            saleRepoMock.Setup(o => o.Get()).Returns(expectedObj.sales.AsQueryable());

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