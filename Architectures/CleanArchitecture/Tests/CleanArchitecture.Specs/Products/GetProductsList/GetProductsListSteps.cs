using Application.Products.Queries.GetProductsList;
using CleanArchitecture.Specs.Common;
using CleanArchitecture.Specs.Common.Data;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CleanArchitecture.Specs.Products.GetProductsList
{
    [Binding]
    [Scope(Feature = "Get Products List")]
    public class GetProductsListSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IGetProductsListQuery _query;

        private ProductModel[] _results;

        public GetProductsListSteps(ScenarioContext scenarioContext,
            IGetProductsListQuery query)
        {
            _scenarioContext = scenarioContext;
            _query = query;
        }

        [When(@"I request a list of products")]
        public async Task WhenIRequestAListOfProducts()
        {
            _results = await _query.ExecuteAsync();
        }

        [Then(@"the products dataset should be returned")]
        public void ThenTheProductsDatasetShouldBeReturned()
        {
            var expectedResults = _scenarioContext.Get<CleanArchitectureDataSet>(ScenarioContextKeys.DataSet).Products
                .Select(o => new ProductModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    UnitPrice = o.Price
                }).ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Compare(expectedObj.expectedResults, _results);
        }

        [Then(@"the following products should be returned:")]
        public void ThenTheFollowingProductsShouldBeReturned(Table table)
        {
            var expectedResults = table.CreateSet<ProductModel>().ToArray();

            var expectedObj = new
            {
                expectedResults
            };

            Compare(expectedObj.expectedResults, _results);
        }

        private void Compare(ProductModel[] expectedArr, ProductModel[] actualArr)
        {
            Assert.AreEqual(expectedArr.Length, actualArr.Length);

            for (var i = 0; i < expectedArr.Length; i++)
            {
                var expected = expectedArr[i];
                var actual = actualArr[i];

                Assert.AreEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.UnitPrice, actual.UnitPrice);
            }
        }
    }
}
