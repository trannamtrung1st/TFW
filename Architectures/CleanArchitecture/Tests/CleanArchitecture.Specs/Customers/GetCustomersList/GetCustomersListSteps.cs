using Application.Customers.Queries.GetCustomersList;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Customers.GetCustomersList
{
    [Binding]
    public class GetCustomersListSteps
    {
        private readonly IGetCustomersListQuery _query;

        private CustomerModel[] _results;

        public GetCustomersListSteps(IGetCustomersListQuery query)
        {
            _query = query;
        }

        [When(@"I request a list of customers")]
        public async Task WhenIRequestAListOfCustomers()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the following customers should be returned:")]
        public void ThenTheFollowingCustomersShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<CustomerModel>().ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Assert.AreEqual(expectedObj.expectedResults.Length, _results.Length);

            for (var i = 0; i < expectedObj.expectedResults.Length; i++)
            {
                var expected = expectedObj.expectedResults[i];
                var actual = _results[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
            }
        }
    }
}
