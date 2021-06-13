using Application.Sales.Queries.GetSalesList;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Sales.GetSalesList
{
    [Binding]
    public class GetSalesListSteps
    {
        private readonly IGetSalesListQuery _query;

        private SalesListItemModel[] _results;

        public GetSalesListSteps(IGetSalesListQuery query)
        {
            _query = query;
        }

        [When(@"I request a list of sales")]
        public async Task WhenIRequestAListOfSales()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the following sales list should be returned:")]
        public void ThenTheFollowingSalesListShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<GetSalesListReturnModel>().ToArray();

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
                Assert.AreEqual(expected.Customer, actual.CustomerName);
                Assert.AreEqual(expected.Date, actual.Date);
                Assert.AreEqual(expected.Employee, actual.EmployeeName);
                Assert.AreEqual(expected.Product, actual.ProductName);
                Assert.AreEqual(expected.Quantity, actual.Quantity);
                Assert.AreEqual(expected.TotalPrice, actual.TotalPrice);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}
