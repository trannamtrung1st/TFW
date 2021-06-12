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

            var uowMock = new Mock<IUnitOfWork>();

            uowMock.Setup(o => o.ToArrayAsync(It.IsAny<IQueryable<CustomerModel>>())).Returns((IQueryable<CustomerModel> inp) =>
            {
                var expected = inp.Select(o => new CustomerModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

                return Task.FromResult(expected);
            });

            var customerRepoMock = new Mock<IRepository<Customer>>();

            customerRepoMock.Setup(o => o.Get()).Returns(customers.AsQueryable());

            var query = new GetCustomersListQuery(uowMock.Object, customerRepoMock.Object);

            var results = await query.ExecuteAsync();

            for (var i = 0; i < customerModels.Length; i++)
            {
                var expected = customerModels[i];
                var actual = results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
            }
        }
    }
}