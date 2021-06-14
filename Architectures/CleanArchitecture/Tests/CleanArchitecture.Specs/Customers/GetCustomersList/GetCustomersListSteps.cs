using Application.Customers.Queries.GetCustomersList;
using CleanArchitecture.Specs.Common;
using CleanArchitecture.Specs.Common.Data;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Customers.GetCustomersList
{
    [Binding]
    [Scope(Feature = "Get Customers List")]
    public class GetCustomersListSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IGetCustomersListQuery _query;

        private CustomerModel[] _results;

        public GetCustomersListSteps(ScenarioContext scenarioContext, IGetCustomersListQuery query)
        {
            _scenarioContext = scenarioContext;
            _query = query;
        }

        [When(@"I request a list of customers")]
        public async Task WhenIRequestAListOfCustomers()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the customers dataset should be returned")]
        public void ThenTheCustomersDatasetShouldBeReturned()
        {
            var expectedResults = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet).Customers
                .Select(o => new CustomerModel
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToArray();

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
