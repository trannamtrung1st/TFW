using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Application.Abstracts.Data;
using Domain.Customers;
using System.Linq;
using Application.Tests.Common.Data;
using Cross.Tests;

namespace Application.Customers.Queries.GetCustomersList.Tests
{
    [TestFixture()]
    public class GetCustomersListQueryTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCase(DataSetKeys.Default)]
        public async Task ExecuteAsyncTest(string dataSetKey)
        {
            var dSet = DataSets.Get(dataSetKey);

            var customerModels = dSet.Customers.Select(o => new CustomerModel
            {
                Id = o.Id,
                Name = o.Name
            }).ToArray();

            var expectedObj = new
            {
                customers = dSet.Customers,
                customerModels
            };

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<CustomerModel>>())).Returns((IQueryable<CustomerModel> inp) =>
            {
                return Task.FromResult(inp.ToArray());
            });

            var customerRepoMock = new Mock<IRepository<Customer>>();

            customerRepoMock.Setup(o => o.Get()).Returns(expectedObj.customers.AsQueryable());

            var query = new GetCustomersListQuery(uowMock.Object, customerRepoMock.Object);

            var results = await query.ExecuteAsync();

            Assert.AreEqual(expectedObj.customerModels.Length, results.Length);

            for (var i = 0; i < expectedObj.customerModels.Length; i++)
            {
                var expected = expectedObj.customerModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
            }
        }
    }
}