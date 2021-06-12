using NUnit.Framework;
using Application.Customers.Queries.GetCustomersList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Application.Abstracts.Data;
using Domain.Customers;
using System.Linq;

namespace Application.Customers.Queries.GetCustomersList.Tests
{
    [TestFixture()]
    public class GetCustomersListQueryTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test()]
        public async Task ExecuteAsyncTest()
        {
            var customers = new Customer[]
            {
                new Customer {Id = 1, Name = "Abc"},
                new Customer {Id = 2, Name = "Xyz"},
            };

            var customerModels = customers.Select(o => new CustomerModel
            {
                Id = o.Id,
                Name = o.Name
            }).ToArray();

            var expectedObj = new
            {
                customers,
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