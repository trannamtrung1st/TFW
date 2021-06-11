using Application.Customers.Queries.GetCustomersList;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Pages.Customers
{
    [TestFixture]
    public class IndexModelTests
    {
        private IndexModel _pageModel;
        private CustomerModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new CustomerModel();

            var queryMock = new Mock<IGetCustomersListQuery>();

            queryMock.Setup(p => p.ExecuteAsync())
                .Returns(Task.FromResult(new[] { _model }));

            _pageModel = new IndexModel(queryMock.Object);
        }

        [Test]
        public async Task TestGetIndexShouldReturnListOfCustomers()
        {
            await _pageModel.OnGetAsync();

            var result = _pageModel.Customers;

            Assert.That(result.Single(), Is.EqualTo(_model));
        }
    }
}
