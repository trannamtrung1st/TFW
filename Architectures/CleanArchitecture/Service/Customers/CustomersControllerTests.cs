using Application.Customers.Queries.GetCustomersList;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Customers
{
    [TestFixture]
    public class CustomersControllerTests
    {
        private CustomersController _controller;
        private CustomerModel _customer;

        [SetUp]
        public void SetUp()
        {
            var queryMock = new Mock<IGetCustomersListQuery>();

            _customer = new CustomerModel();

            queryMock.Setup(p => p.ExecuteAsync())
                .Returns(Task.FromResult(new[] { _customer }));

            _controller = new CustomersController(queryMock.Object);
        }

        [Test]
        public async Task TestGetCustomersListShouldReturnListOfCustomers()
        {
            var results = await _controller.GetCustomersListAsync();

            Assert.Contains(_customer, results.ToArray());
        }
    }
}
