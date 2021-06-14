using Application.Sales.Queries.GetSalesList;
using CleanArchitecture.Specs.Common;
using CleanArchitecture.Specs.Common.Data;
using FluentAssertions;
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
    [Scope(Feature = "Get Sales List")]
    public class GetSalesListSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IGetSalesListQuery _query;

        private SalesListItemModel[] _results;

        public GetSalesListSteps(ScenarioContext scenarioContext,
            IGetSalesListQuery query)
        {
            _scenarioContext = scenarioContext;
            _query = query;
        }

        [When(@"I request a list of sales")]
        public async Task WhenIRequestAListOfSales()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the sales dataset should be returned")]
        public void ThenTheSalesDatasetShouldBeReturned()
        {
            var expectedResults = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet).Sales
                .Select(o => new GetSalesListReturnModel
                {
                    Customer = o.Customer.Name,
                    Date = o.Date,
                    Employee = o.Employee.Name,
                    Id = o.Id,
                    Product = o.Product.Name,
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    UnitPrice = o.UnitPrice
                }).ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Compare(expectedObj.expectedResults, _results);
        }

        [Then(@"the following sales list should be returned:")]
        public void ThenTheFollowingSalesListShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<GetSalesListReturnModel>().ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Compare(expectedObj.expectedResults, _results);
        }

        private void Compare(GetSalesListReturnModel[] expectedArr, SalesListItemModel[] actualArr)
        {
            expectedArr.Length.Should().Be(_results.Length);

            for (var i = 0; i < expectedArr.Length; i++)
            {
                var expected = expectedArr[i];
                var actual = actualArr[i];

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
