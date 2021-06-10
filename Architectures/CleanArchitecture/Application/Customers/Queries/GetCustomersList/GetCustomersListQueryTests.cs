using Application.Abstracts.Data;
using Domain.Customers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customers.Queries.GetCustomersList
{
    [TestFixture]
    public class GetCustomersListQueryTests
    {
        private GetCustomersListQuery _query;
        private Customer _customer;

        private const int Id = 1;
        private const string Name = "Customer 1";

        [SetUp]
        public void SetUp()
        {
            _customer = new Customer()
            {
                Id = Id,
                Name = Name
            };

            var customerArr = new[] { _customer };
            var customerModelArr = new[]
            {
                new CustomerModel
                {
                    Id = _customer.Id,
                    Name = _customer.Name
                }
            };

            var queryable = customerModelArr.AsQueryable();
            var repoMock = new Mock<IRepository<Customer>>();
            var uowMock = new Mock<IUnitOfWork>();

            repoMock.Setup(p => p.Get())
                .Returns(() => customerArr.AsQueryable());

            uowMock.Setup(p => p.ToArrayAsync(queryable))
                .Returns(() => Task.FromResult(customerModelArr));

            _query = new GetCustomersListQuery(uowMock.Object, repoMock.Object);
        }

        [Test]
        public async Task TestExecuteShouldReturnListOfCustomers()
        {
            var results = await _query.ExecuteAsync();

            var result = results.Single();

            Assert.AreEqual(result.Id, Id);
            Assert.AreEqual(result.Name, Name);
        }
    }
}
